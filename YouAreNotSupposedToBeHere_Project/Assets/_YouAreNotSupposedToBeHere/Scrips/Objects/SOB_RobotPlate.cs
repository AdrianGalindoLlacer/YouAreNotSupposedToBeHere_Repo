
using UnityEngine;

public class SOB_RobotPlate : MonoBehaviour
{

    [SerializeField] Animator DoorRight;
    [SerializeField] Animator DoorLeft;

    public void pick()
    {
        DoorRight.SetBool("Open", false);
        DoorLeft.SetBool("Open", false);
        gameObject.SetActive(false);
    }
}
