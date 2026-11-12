using UnityEngine;
using UnityEngine.InputSystem;

public class PSC_PlayerController2 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    public float Speed = 3f;
    [SerializeField] float MaxForce = 1f; //Fuerza de arranque
    [SerializeField] Vector2 moveInput;
    [SerializeField] float RotationSpeed = 10f; // Velocidad de rotacion
    [SerializeField] LayerMask InteractLayer; //Capa donde el interactor buscara colisiones
    [SerializeField] EN_IntTags interact = EN_IntTags.Null; //Variable que guarda los tipos de colisiones con las que interactua el interactor
    [SerializeField] float RadioInter = 0.1f; //Radio de busqueda del interactor
    public bool Block; // Bool que bloque el movimiento por completo del jugador

    [Header("mechanics")]
    public int Ammo; //Municion del arma total
    [SerializeField] bool CanShoot; //Bool que impide al jugador disparar
    public bool Hide; // bool que indica que el jugador esta escondido
    public EN_IntTags Interact => interact;
    private Collider interactCollider; // Almacena el collider del interactor
    private bool Dcount;

    [Header("Referencias a componentes")]
    [SerializeField] Transform Interactor; //almacena el transform del interactor
    [SerializeField] Transform ShootPoint; //almacena el transform de donde salen las balas
    [SerializeField] GameObject Bullet; //almacena las balas
    [SerializeField] MAN_Health Health;
    [SerializeField] MAN_SceneManager Manager;
    [SerializeField] MAN_Player2SoundController soundC;
    private bool notwsound;
    Rigidbody rb; //almacena el rigidbody del jugador
    Animator anim; //almacena el animator del jugador



    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //AnimatorManager();
        Transform origen = Interactor != null ? Interactor : transform;

        Collider[] collisions = Physics.OverlapSphere(origen.position, RadioInter, InteractLayer);

        interact = EN_IntTags.Null;
        interactCollider = null;
        float MinDistance = float.MaxValue;

        foreach (Collider col in collisions)
        {
            EN_IntTags foundtag = ObtainTag(col);

            if (foundtag == EN_IntTags.Null) continue;
            float distance = (col.transform.position - origen.position).sqrMagnitude;

            if (distance < MinDistance)
            {
                MinDistance = distance;
                interact = foundtag;
                interactCollider = col;
            }
        }
    }

    private EN_IntTags ObtainTag(Collider col)
    {
        if (col.CompareTag("Slide")) return EN_IntTags.Slide;
        if (col.CompareTag("Buttons")) return EN_IntTags.Buttons;
        if (col.CompareTag("Hideout")) return EN_IntTags.Hideout;
        return EN_IntTags.Null;
    }

    private void OnDrawGizmosSelected()
    {
        Transform origen = Interactor != null ? Interactor : transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origen.position, RadioInter);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y) * Speed;
        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, MaxForce);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        Rotate();
    }

    void Rotate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }

    void Shoot()
    {
            soundC.StopSound();
            soundC.playShoot();
            Instantiate(Bullet, ShootPoint.position, transform.rotation);
            Ammo--;
            //SOB_Bullet controller = Bullet.GetComponent<SOB_Bullet>();
            //controller.Move = true;
    }

    //private void AnimatorManager()
    //{
    //gestion del animator
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Guard"))
        {
            if (!Health.Dcount)
            {
                Health.Health = 0;
                Dcount = true;
                anim.SetTrigger("GameOver");
                soundC.StopSound();
                soundC.playGO();
                Manager.gameover();
            }
        }
    }

    public void unblock()
    {
        Block = !Block;
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        if (Block) return;
        moveInput = context.ReadValue<Vector2>();
        if (context.performed)
        {
            anim.SetBool("Move", true);
            if (!notwsound) soundC.playWalk();

        }
        else if (context.canceled)
        {
            anim.SetBool("Move", false);
            //Habria que comprobar si el sonido que se esta haciendo en el manager es el de walk para si lo es que lo detenga
            soundC.StopSound();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!Block)
        {
        if (Interact == EN_IntTags.Buttons)
        {
                Block = true;
                anim.SetTrigger("Button");
                if (interactCollider.TryGetComponent(out SOB_Button button))
                {
                    button.Press();
                }
            }
        else if (Interact == EN_IntTags.Hideout)
        {
            Block = true;
            Hide = true;
            anim.SetTrigger("Hide");
            soundC.StopSound();
            soundC.playHide();
            if (interactCollider.TryGetComponent(out SOB_Hideout Hideout))
            {
                Hideout.Hiding = true;
            }
        }
        else if (Interact == EN_IntTags.Slide)
        {
            if (interactCollider.TryGetComponent(out SOB_Slide Slide))
            {
                Slide.Slide();
            }
        }
        }
        else if (Hide)
        {
            Hide = false;
            Block = true;
            anim.SetTrigger("UnHide");
        }
    }

    public void Onshot(InputAction.CallbackContext context)
    {
        if (Block) return;
        if (!context.performed) return;

        if (Ammo <= 0)
        {
            Block = true;
            anim.SetTrigger("NoAmmo");
        }
        else
        {
            Block = true;
            anim.SetTrigger("Shoot");
        }
    }

    #endregion
}