using Unity.VisualScripting;
using UnityEngine;

public class ENE_Damage : MonoBehaviour
{
    private int count = 0;
    [SerializeField] float damage = 0.01f;
    [SerializeField] GameObject Alert;
    [SerializeField] MAN_SceneManager Manager;
    [SerializeField] MAN_Health Health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        { 
            count++;
            Alert.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health.Health -= damage;
            if (Health.Health <= 0 && !Health.Dcount)
            {
                Health.Dcount = true;
                Manager.gameover();
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
                Alert.SetActive(false);
            }
        }
    }
}
