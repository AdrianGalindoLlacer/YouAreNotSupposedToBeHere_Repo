using UnityEngine;
using UnityEngine.InputSystem;

public class PSC_PlayerController2 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    [SerializeField] float Speed = 3f;
    [SerializeField] float MaxForce = 1f;
    [SerializeField] Vector2 moveInput;
    [SerializeField] float RotationSpeed = 10f;
    [SerializeField] LayerMask InteractLayer;
    [SerializeField] EN_IntTags interact = EN_IntTags.Null;
    [SerializeField] float RadioInter = 0.1f;
    public EN_IntTags Interact => interact;
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
    [SerializeField] Transform Interactor;
    [SerializeField] Transform ShootPoint;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject SBullet;
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

    private void Update()
    {
        Transform origen = Interactor != null ? Interactor : transform;

        Collider[] collisions = Physics.OverlapSphere(origen.position, RadioInter, InteractLayer);

        interact = EN_IntTags.Null;
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
        Transform origen = Interactor !=null ? Interactor : transform;
        Gizmos.color = Color.yellow;
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
            Vector3 Direction = transform.forward;
            Vector3 position = transform.position + Direction * ShootDistance;

            GameObject Projectile = Instantiate(SBullet, position, Quaternion.LookRotation(Direction));

            Rigidbody rbb = Projectile.GetComponent<Rigidbody>();
            rbb.AddForce(Direction * ShootForce, ForceMode.VelocityChange);
        }
        else 
        {
            Vector3 Direction = transform.forward;
            Vector3 position = transform.position + Direction * ShootDistance;

            GameObject Projectile = Instantiate(Bullet, position, Quaternion.LookRotation(Direction));

            Rigidbody rbb = Projectile.GetComponent<Rigidbody>();
            rbb.AddForce(Direction * ShootForce, ForceMode.VelocityChange);
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
            Debug.Log("Boton");

        }
        else if (Hide)
        {
            //Hide = false;
            Debug.Log("NEscondite");
            //anim.SetBool("Hide", false);
        }
        else if (Interact == EN_IntTags.Hideout)
        {
            //Hide = true;
            Debug.Log("Escondite");
            //Block = true;
            //anim.SetBool("Hide", true);
        } 
        else if (Interact == EN_IntTags.Slide)
        {
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
