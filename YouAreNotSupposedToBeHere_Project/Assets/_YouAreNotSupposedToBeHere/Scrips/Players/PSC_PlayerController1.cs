using UnityEngine;
using UnityEngine.InputSystem;

public class PSC_PlayerController1 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    [SerializeField] float Speed = 3f;
    [SerializeField] float MaxForce = 1f; //Fuerza de arranque
    [SerializeField] Vector2 moveInput;
    [SerializeField] float RotationSpeed = 10f; // Velocidad de rotacion
    [SerializeField] LayerMask InteractLayer; //Capa donde el interactor buscara colisiones
    [SerializeField] EN_IntTags interact = EN_IntTags.Null; //Variable que guarda los tipos de colisiones con las que interactua el interactor
    [SerializeField] float RadioInter = 0.1f; //Radio de busqueda del interactor
    [SerializeField]private bool Block; // Bool que bloque el movimiento por completo del jugador

    [Header("mechanics")]
    public bool HoldRobot = true; //bool que guarda si el jugador lleva el robot
    bool WalkRobot; //Bool que guarda si el robot esta en walk
    [SerializeField] float damage = 0.01f;
    private bool Dcount;
    public EN_IntTags Interact => interact;
    private Collider interactCollider; // Almacena el collider del interactor


    [Header("Referencias a componentes")]
    [SerializeField] Transform Interactor; //transform del interactor
    [SerializeField] GameObject Robot; //el robot que el jugador lleva
    [SerializeField] Transform Spawn; //transform donde el robot aparecera
    [SerializeField] GameObject RobotStand;
    [SerializeField] GameObject Origin;
    [SerializeField] GameObject RobotThrow; //El robot que el jugador deja
    [SerializeField] MAN_Health Health;
    [SerializeField] MAN_SceneManager Manager;
    [SerializeField] MAN_Player1SoundController soundC;
    private bool notwsound;
    SOB_Robot controller;
    Rigidbody rb;
    Animator anim;



    #endregion

    private void Awake()
    {
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

    void Lay()
    {
        HoldRobot = false;
        if (!WalkRobot)
        {
            soundC.StopSound();
            soundC.playLay();
            Robot.SetActive(false);
            RobotThrow.transform.SetPositionAndRotation(Spawn.position, transform.rotation);
            controller.soundenabled = true;
        }
        else
        {
            soundC.StopSound();
            soundC.playLay();
            Robot.SetActive(false);
            RobotThrow.transform.SetPositionAndRotation(Spawn.position, transform.rotation);
            controller.Walk = true;
            controller.soundenabled = true;
        }
    }

    private void Grab()
    {
        soundC.StopSound();
        soundC.playGrab();
        controller.Walk = false;
        RobotStand.SetActive(false);
        RobotThrow.transform.position = Origin.transform.position;
        Robot.SetActive(true);
        HoldRobot = true;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Vision"))
        {
            Health.Health -= damage;
            if (Health.Health <= 0 && !Dcount)
            {
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
        //if (Block)
        //{
            if (Interact == EN_IntTags.Buttons)
            {
                Block = true;
                if (interactCollider.TryGetComponent(out SOB_Button button))
                {
                    Debug.Log("Boton");
                    button.Press();
                    anim.SetTrigger("Button");
                }
            }
            else if (Interact == EN_IntTags.Robot)
            {
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
            else if (HoldRobot)
            {
                Block = true;
                Debug.Log("WalkMode");
                WalkRobot = !WalkRobot;
                anim.SetTrigger("Change");
            }
        //}
    }

    public void OnLay(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!HoldRobot)
        {
            anim.SetTrigger("NoRobot");
            Block = true;
        }
        else
        {
            anim.SetTrigger("Lay");
            Block = true;
        }
    }

    #endregion
}