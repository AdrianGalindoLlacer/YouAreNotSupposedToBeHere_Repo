using UnityEngine;
using UnityEngine.UI;

public class UI_PopUps : MonoBehaviour
{
    [SerializeField] private GameObject popUp;

    private int count;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            count++;
            if (count > 0)
            {
                popUp.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            count--;
            if (count == 0)
            {
                popUp.SetActive(false);
            }
        }
    }
}