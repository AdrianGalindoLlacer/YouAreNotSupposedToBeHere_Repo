using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PSC_PlayerController2 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    [SerializeField] float Speed = 3f;
    [SerializeField] float SprintSpeed = 4f;
    [SerializeField] float CrouchSpeed = 2f;
    [SerializeField] float MaxForce = 1f;
    [SerializeField] Vector2 moveInput;
    [SerializeField] float RotationSpeed = 10f;

    [Header("mechanics")]
    [SerializeField] Transform ShootPoint;
    [SerializeField] int Ammo;
    [SerializeField] int SpecialAmmo;
    [SerializeField] bool Sprinting;
    [SerializeField] bool Crouching;

    [Header("Movidas que traer")]
    Rigidbody rb;
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
        //Esto se pondra en el player controller1 Cursor.lockState = CursorLockMode.Locked;
        //Esto se pondra en el player controller1 Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y);
        targetVelocity *= Sprinting ? SprintSpeed : Crouching ? CrouchSpeed : Speed; //Exclusivo del jugador 2
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

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    #endregion
}
