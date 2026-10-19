using UnityEngine;

public class ENE_Guard : MonoBehaviour
{
    Animator anim;
    [SerializeField] float speed = 2f;
    [SerializeField] bool patrol = false;
    [SerializeField] bool chase = true;
    [SerializeField] GameObject RobotHold;
    [SerializeField] GameObject RobotTarget;
    [SerializeField] GameObject RobotStand;
    [SerializeField] GameObject Vision;
    private SOB_Robot Target;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Target = RobotTarget.GetComponent<SOB_Robot>();
    }

    private void Update()
    {
        if (Target.sound == true && chase == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, RobotTarget.transform.position, speed * Time.deltaTime);
            if (transform.position == RobotTarget.transform.position)
            {
                Vision.SetActive(false);
                anim.SetBool("Pick", true);
                chase = false;
            }
        }
    }
    private void pick()
    {
        Target.sound = false;
        RobotTarget.SetActive(false);
        anim.SetBool("Pick", false);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Robot") && patrol)
        {
            patrol = false;
            Vision.SetActive(false);
            anim.SetTrigger("Trip");
            anim.SetBool("Pick", true);
        }
    }
}
