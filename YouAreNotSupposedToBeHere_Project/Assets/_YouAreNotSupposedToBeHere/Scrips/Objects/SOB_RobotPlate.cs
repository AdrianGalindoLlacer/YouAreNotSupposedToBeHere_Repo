
using UnityEngine;

public class SOB_RobotPlate : MonoBehaviour
{

    [SerializeField] SOB_PresurePRob Plate;

    public void pick()
    {
        Plate.count--;
        Plate.Press();
        gameObject.SetActive(false);
    }
}
