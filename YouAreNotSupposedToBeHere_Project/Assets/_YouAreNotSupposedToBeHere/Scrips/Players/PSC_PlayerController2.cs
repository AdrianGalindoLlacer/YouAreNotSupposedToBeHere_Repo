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
    public EN_IntTags Interact => interact;
    private Collider interactCollider; // Almacena el collider del interactor
    private bool Block; // Bool que bloque el movimiento por completo del jugador

    [Header("mechanics")]
    public int Ammo; //Municion del arma total
    public int SpecialAmmo; //PostIts que tiene el player
    private bool PostIt; //Bool que activa el disparar postIts
    [SerializeField] bool CanShoot; //Bool que impide al jugador disparar
    [SerializeField] bool Hide; // bool que indica que el jugador esta escondido
    [SerializeField] float damage = 0.01f;

    [Header("Referencias a componentes")]
    [SerializeField] Transform Interactor; //almacena el transform del interactor
    [SerializeField] Transform ShootPoint; //almacena el transform de donde salen las balas
    [SerializeField] GameObject Bullet; //almacena las balas
    [SerializeField] GameObject SBullet; //almacena las balas con PostIt
    [SerializeField] MAN_Health Health;
    [SerializeField] MAN_SceneManager Manager;
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
        if (PostIt)
        {
            Instantiate(SBullet, ShootPoint.position, transform.rotation);
            SpecialAmmo--;
            Ammo--;
            SOB_Bullet controller = Bullet.GetComponent<SOB_Bullet>();
            controller.Move = true;
            Block = false;
        }
        else
        {
            Instantiate(Bullet, ShootPoint.position, transform.rotation);
            Ammo--;
            SOB_Bullet controller = SBullet.GetComponent<SOB_Bullet>();
            controller.Move = true;
            Block = false;
        }
    }

    //private void AnimatorManager()
    //{
    //gestion del animator
    //}


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Vision"))
        {
            Health.Health -= damage;
            if (Health.Health <= 0)
            {
                Manager.Restart();
            }
        }
    }
    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        if (Block) return;
        moveInput = context.ReadValue<Vector2>();
        //if (context.performed)
        //{
        //    anim.SetBool("Move", true);
        //
        //}
        //else if (context.canceled)
        //{
        //    anim.SetBool("Move", false);
        //}
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Interact == EN_IntTags.Buttons)
        {
            if (interactCollider.TryGetComponent(out SOB_Button button))
            {
                Debug.Log("Boton");
                button.Press();
            }
        }
        else if (Hide)
        {
            Hide = false;
            Block = false;
            Debug.Log("NEscondite");
            //anim.SetBool("Hide", false);
        }
        else if (Interact == EN_IntTags.Hideout)
        {
            Hide = true;
            Debug.Log("Escondite");
            Block = true;
            //anim.SetBool("Hide", true);
            if (interactCollider.TryGetComponent(out SOB_Hideout Hideout))
            {
                Hideout.Hiding = true;
            }
        }
        else if (Interact == EN_IntTags.Slide)
        {
            if (interactCollider.TryGetComponent(out SOB_Slide Slide))
            {
                //anim.SetTrigger("Slide");
                Slide.Slide();
            }
            Debug.Log("Tobogan");
        }
        else if (!PostIt && SpecialAmmo > 0)
        {
            Debug.Log("Postit");
            PostIt = true;
        }
        else
        {
            Debug.Log("NPostit");
            PostIt = false;
        }
    }

    public void Onshot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Ammo <= 0)
        {
            //anim.SetTrigger("NoAmmo");
            Block = true;
        }
        else
        {

            //anim.SetTrigger("Shoot");
            Block = true;
            Shoot();
        }
    }

    #endregion
}