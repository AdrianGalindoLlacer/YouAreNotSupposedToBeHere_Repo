using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PSC_PlayerController2 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    [SerializeField] float Speed = 3f;
    [SerializeField] float MaxForce = 1f;
    [SerializeField] Vector2 moveInput;
    [SerializeField] float RotationSpeed = 10f;
    [SerializeField] - InteractCheck;
    private bool Block;

    [Header("mechanics")]
    [SerializeField] Transform ShootPoint;
    [SerializeField] int Ammo;
    [SerializeField] int SpecialAmmo;
    private bool PostIt;
    [SerializeField] - Bullet;  
    [SerializeField] - SBullet; 
    [SerializeField] bool CanShoot;
    [SerializeField] bool Hide; 

    [Header("Referencias a componentes")]
    Rigidbody rb;
    Animator anim;
    [SerializeField] Transform Interactor;

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
        if (Block)
        {
            return;
        }
        else 
        { 
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, MaxForce);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        Rotate();
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

    void Shoot()
    {
        if (PostIt == true)
        {

        }
        else 
        {
            
        }
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (InteractCheck == Buttons)
        {


        }
        else if (Hide == true)
        {
            //Hide = false;
            anim.SetBool("Hide", false);
        }
        else if (InteractCheck == Hideout)
        {
            //Hide = true;
            Block = true;
            anim.SetBool("Hide", true);
        } 
        else if (InteractCheck == Slide)
        {

        }
        else if (PostIt == false && SpecialAmmo > 0)
        {
            PostIt = true;
        }
        else
        {
            PostIt = false;
        }
    }

    public void Onshot(InputAction.CallbackContext context)
    {
        if (Ammo <= 0)
        {
            anim.SetTrigger("NoAmmo");
            Block = true;
        }
        else
        {
            anim.SetTrigger("Shoot");
            Block = true;
        }
    }

    #endregion
}
