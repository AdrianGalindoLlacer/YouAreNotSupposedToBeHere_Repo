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
    [SerializeField] - Interact;
    private bool Block;

    [Header("mechanics")]
    [SerializeField] int Ammo;
    [SerializeField] int SpecialAmmo;
    private bool PostIt;
    [SerializeField] bool CanShoot;
    [SerializeField] bool Hide;
    [SerializeField] float ShootForce;
    [SerializeField] float ShootDistance;
    [SerializeField] float ShootDuration;

    [Header("Referencias a componentes")]
    Rigidbody rb;
    Animator anim;
    [SerializeField] Transform Interactor;
    [SerializeField] Transform ShootPoint;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject SBullet;


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

    private void Update()
    {
        
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
            Vector3 Direction = transform.forward;
            Vector3 position = transform.position + Direction * ShootDistance;

            GameObject Projectile = Instantiate(SBullet, position, Quaternion.LookRotation(Direction));

            Rigidbody rbb = Projectile.GetComponent<Rigidbody>();
            rbb.AddForce(Direction * ShootForce, ForceMode.VelocityChange);


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
        if (Interact == Buttons)
        {


        }
        else if (Hide == true)
        {
            //Hide = false;
            anim.SetBool("Hide", false);
        }
        else if (Interact == Hideout)
        {
            //Hide = true;
            Block = true;
            anim.SetBool("Hide", true);
        } 
        else if (Interact == Slide)
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
