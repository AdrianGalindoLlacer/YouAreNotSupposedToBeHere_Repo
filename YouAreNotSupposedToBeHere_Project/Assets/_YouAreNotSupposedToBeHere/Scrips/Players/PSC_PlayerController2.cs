using UnityEngine;
using UnityEngine.InputSystem;

public class PSC_PlayerController2 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    [SerializeField] float Speed = 3f;
    [SerializeField] float SprintSpeed = 6f;
    [SerializeField] float MaxForce = 1f; //Fuerza de arranque
    [SerializeField] float RotationSpeed = 10f; // Velocidad de rotacion
    [SerializeField] LayerMask InteractLayer; //Capa donde el interactor buscara colisiones
    [SerializeField] EN_IntTags interact = EN_IntTags.Null; //Variable que guarda los tipos de colisiones con las que interactua el interactor
    [SerializeField] float RadioInter = 0.1f; //Radio de busqueda del interactor
    Vector2 moveInput;
    public bool Block; // Bool que bloque el movimiento por completo del jugador
    private bool Run;

    [Header("mechanics")]
    [SerializeField] bool CanShoot; //Bool que impide al jugador disparar
    public bool Hide; // bool que indica que el jugador esta escondido
    public EN_IntTags Interact => interact;
    private Collider interactCollider; // Almacena el collider del interactor

    [Header("Referencias a componentes")]
    [SerializeField] private Transform Interactor; //almacena el transform del interactor
    private MAN_Health Health;
    private MAN_Player2SoundController soundC;
    private bool notwsound;
    private Rigidbody rb; //almacena el rigidbody del jugador
    private Animator anim; //almacena el animator del jugador
    private PSC_PlayerShoot2 shoot; //almacena el script de disparo del jugador



    #endregion

    private void Awake()
    {
        Health = FindFirstObjectByType<MAN_Health>();
        soundC = FindFirstObjectByType<MAN_Player2SoundController>();
        shoot = GetComponent<PSC_PlayerShoot2>();
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

    //private void AnimatorManager()
    //{
    //gestion del animator
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Guard"))
        {
            Health.Health = 0;
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
        if (Run)
        {
            anim.SetBool("Run", true);
        }
        if (context.performed)
        {
            anim.SetBool("Move", true);
            if (!notwsound) soundC.playWalk();

        }
        else if (context.canceled)
        {
            anim.SetBool("Run", false);
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

    #endregion
}