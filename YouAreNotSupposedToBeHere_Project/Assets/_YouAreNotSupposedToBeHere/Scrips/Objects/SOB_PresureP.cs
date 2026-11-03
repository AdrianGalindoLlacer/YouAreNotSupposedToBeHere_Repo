using System;
using UnityEngine;

public class SOB_PresureP : MonoBehaviour
{
    Animator anim;
    [SerializeField] int count = 0; //Variable que almacena la cantidad de colliders que tiene encima
    [SerializeField] Animator DoorRight;
    [SerializeField] Animator DoorLeft;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Press()
    {
        if (count > 0)
        {
            anim.SetBool("Press", false);
            DoorRight.SetBool("Open", false);
            DoorLeft.SetBool("Open", false);
        }
        else
        {
            anim.SetBool("Press", true);
            DoorRight.SetBool("Open", true);
            DoorLeft.SetBool("Open", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Box"))
        {
            count++;
            Press();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Box"))
        {
            count--;
            Press();
        }
    }
}
