using UnityEngine;

public class SOB_RobotPassage : MonoBehaviour
{
    [SerializeField] GameObject Robot; //Almacena el robot con scipt
    [SerializeField] Transform Passage; //Almacena el objetivo del robot al que sera transportado

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
            Robot.SetActive(false);
            Robot.transform.position = Passage.position;
            Robot.transform.rotation = Passage.rotation;
            Robot.SetActive(true);
        }
    }
}
