using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PSC_PlayerController1 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    [SerializeField] float Speed = 3f;
    [SerializeField] float SprintSpeed = 6f;
    [SerializeField] float MaxForce = 1f; //Fuerza de arranque
    [SerializeField] Vector2 moveInput;
    [SerializeField] float RotationSpeed = 10f; // Velocidad de rotacion
    [SerializeField] LayerMask InteractLayer; //Capa donde el interactor buscara colisiones
    [SerializeField] EN_IntTags interact = EN_IntTags.Null; //Variable que guarda los tipos de colisiones con las que interactua el interactor
    [SerializeField] float RadioInter = 0.1f; //Radio de busqueda del interactor
    public bool Block; // Bool que bloque el movimiento por completo del jugador
    private bool Run;
    private bool notwsound;

    [Header("mechanics")]
    public EN_IntTags Interact => interact;
    private Collider interactCollider; // Almacena el collider del interactor


    [Header("Referencias a componentes")]
    [SerializeField] private Transform Interactor; //transform del interactor
    [SerializeField] GameObject RobotStand;
    [SerializeField] private GameObject Robot; //el robot que el jugador lleva
    private GameObject Origin;
    private GameObject RobotThrow; //El robot que el jugador deja
    private MAN_Health Health;
    private MAN_Player1SoundController soundC;
    private PSC_PlayerShoot1 shoot;
    private SOB_Robot controller;
    private Rigidbody rb;
    private Animator anim;



    #endregion

    private void Awake()
    {
        Origin = GameObject.Find("SK_RobotScripted");
        RobotThrow = FindFirstObjectByType<SOB_Robot>().gameObject;
        Health = FindFirstObjectByType<MAN_Health>();
        soundC = FindFirstObjectByType<MAN_Player1SoundController>();
        shoot = GetComponent<PSC_PlayerShoot1>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        controller = RobotThrow.GetComponent<SOB_Robot>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Health.Health <= 0 && !Health.Dcount)
        {
            anim.SetTrigger("GameOver");
            Health.Dcount = true;
        }
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
        if (col.CompareTag("Robot")) return EN_IntTags.Robot;
        if (col.CompareTag("Buttons")) return EN_IntTags.Buttons;
        if (col.CompareTag("Stand")) return EN_IntTags.Stand;
        if (col.CompareTag("Box")) return EN_IntTags.Box;
        return EN_IntTags.Null;
    }

    private void OnDrawGizmosSelected()
    {
        Transform origen = Interactor != null ? Interactor : transform;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origen.position, RadioInter);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (Block) return;

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y) * Speed;
        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, MaxForce);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        Rotate();
    }

    private void Grab()
    {
        soundC.StopSound();
        soundC.playGrab();
        controller.Walk = false;
        RobotStand.SetActive(false);
        RobotThrow.transform.position = Origin.transform.position;
        Robot.SetActive(true);
        shoot.HoldRobot = true;
        controller.soundenabled = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            notwsound = true;
            soundC.StopSound();
            anim.SetBool("Push", true);
            soundC.playPush();
        }
        if (collision.gameObject.CompareTag("Guard"))
        {
            Health.Health = 0;
        }
}

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            soundC.StopSound();
            notwsound = false;
            anim.SetBool("Push", false);
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
                    Debug.Log("Boton");
                    button.Press();
                }
            }
            else if (Interact == EN_IntTags.Robot)
            {
                if (interactCollider.TryGetComponent(out SOB_RobotPlate RobotPlate))
                {
                    RobotPlate.pick();
                }
                Block = true;
                Debug.Log("Robot");
                anim.SetTrigger("GrabF");


            }
            else if (Interact == EN_IntTags.Stand)
            {
                Block = true;
                Debug.Log("Robot");
                anim.SetTrigger("GrabS");

            }
            else if (shoot.HoldRobot)
            {
                Block = true;
                Debug.Log("WalkMode");
                shoot.WalkRobot = !shoot.WalkRobot;
                anim.SetTrigger("Change");
            }
        }
    }

    #endregion
}