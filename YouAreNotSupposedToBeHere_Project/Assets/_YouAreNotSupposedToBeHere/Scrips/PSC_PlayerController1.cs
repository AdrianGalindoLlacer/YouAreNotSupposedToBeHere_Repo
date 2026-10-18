using UnityEngine;
using UnityEngine.InputSystem;

public class PSC_PlayerController1 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    [SerializeField] float Speed = 3f;
    [SerializeField] float MaxForce = 1f;
    [SerializeField] Vector2 moveInput;
    [SerializeField] float RotationSpeed = 10f;
    [SerializeField] LayerMask InteractLayer;
    [SerializeField] EN_IntTags interact = EN_IntTags.Null;
    [SerializeField] float RadioInter = 0.5f;
    public EN_IntTags Interact => interact;
    private Collider interactCollider;
    private bool Block = false;

    [Header("mechanics")]
    bool HoldRobot = true;
    bool WalkRobot;


    [Header("Referencias a componentes")]
    [SerializeField] Transform Interactor;
    [SerializeField] GameObject Robot;
    [SerializeField] Transform Spawn;
    [SerializeField] GameObject RobotThrow;
    Rigidbody rb;
    Transform tr;
    Animator anim;



    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        foreach(Collider col in collisions)
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
        if (col.CompareTag("Box")) return EN_IntTags.Box;
        return EN_IntTags.Null;
    }

    private void OnDrawGizmosSelected()
    {
        Transform origen = Interactor !=null ? Interactor : transform;
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
        if (!WalkRobot)
        {
            Instantiate(RobotThrow, Spawn.position, tr.rotation);
        }
        else
        {
            GameObject InteractableR = Instantiate(RobotThrow, Spawn.position, tr.rotation);
            SOB_Robot controller = InteractableR.GetComponent<SOB_Robot>();
            controller.Walk = true;
        }
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


    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Interact == EN_IntTags.Buttons)
        {
            //if (interactCollider.TryGetComponent(out SOB_Button button))
            //{
                Debug.Log("Boton");
                //button.Press();
            //}
        }
        else if (Interact == EN_IntTags.Box)
        {
            //Block = true;
            Debug.Log("Caja");
        } 
        else if (Interact == EN_IntTags.Robot)
        {
            Debug.Log("Robot");
            //anim.SetTrigger("Grab");

        }
        else if (!WalkRobot && HoldRobot)
        {
            Debug.Log("WalkMode");
            WalkRobot = true;
        }
        else
        {
            Debug.Log("NPostit");
            WalkRobot = false;
        }
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
