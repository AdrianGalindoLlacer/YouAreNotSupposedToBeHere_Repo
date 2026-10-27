using System;
using UnityEngine;

public class SOB_PresureP : MonoBehaviour
{
    //Animator anim;
    [SerializeField] int count = 0; //Variable que almacena la cantidad de colliders que tiene encima
    //[SerializeField] Animator Door; // Animator de la puerta para mandarle señales
    [SerializeField] GameObject DoorRight; 
    [SerializeField] GameObject DoorLeft; 


    //private void Awake()
    //{
    //    anim = GetComponent<Animator>();
    //}

    private void Press()
    {
        if (count > 0)
        {
            DoorLeft.SetActive(false);
            DoorRight.SetActive(false);
            //anim.SetBool("Press", false);
            //Door.SetTrigger("Close");
        }
        else
        {
            DoorLeft.SetActive(true);
            DoorRight.SetActive(true);
            //anim.SetBool("Press", true);
            //Door.SetTrigger("Open");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            count++;
            Press();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            count--;
            Press();
        }
    }
}
