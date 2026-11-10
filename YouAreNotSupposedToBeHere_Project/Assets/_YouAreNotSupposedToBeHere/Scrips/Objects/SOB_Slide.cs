using System.Collections;
using UnityEngine;

public class SOB_Slide : MonoBehaviour
{

    [SerializeField] private Animator enter;
    //[SerializeField] private Animator exit;
    [SerializeField] Transform goal; // Variable que guarda el transform del empty en el que el jugador se teletransportara
    [SerializeField] GameObject Player; // Variable que guarda el player que se transportara
    [SerializeField] GameObject fakePlayerEnter;
    //[SerializeField] GameObject fakePlayerExit;
    [SerializeField] float Timer = 8f; // duracion de la rutina
    [SerializeField] private MAN_SlideSoundController soundC;

    private Coroutine slideRoutine;

    //private void Awake()
    //{
    //    enter = fakePlayerEnter.GetComponent<Animator>();
    //    exit = fakePlayerExit.GetComponent<Animator>();
    //}

    public void Transport()
    {

        if (Player.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        Player.transform.position = goal.position;
        Physics.SyncTransforms();
    }

    public void eventEnt()
    {
        soundC.playEnter();
        slideRoutine = StartCoroutine(SlideTimerRoutine());
    }

    public void eventExi()
    {
        //fakePlayerExit.SetActive(false);
        Player.SetActive(true);
    }

    public void Slide()
    {
        Player.SetActive(false);
        fakePlayerEnter.SetActive(true);
        enter.SetTrigger("Slide");//esta animacion tendra un evento de animacion que hara que se active EventEnt cuando la rutina acabe
    }

    private IEnumerator SlideTimerRoutine()
    {
        yield return new WaitForSeconds(Timer);
        eventEnt();
        fakePlayerEnter.SetActive(false);
        //fakePlayerExit.SetActive(true);
        Player.SetActive(true);
        //exit.SetTrigger("SlideExit");//esta animacion tendra un evento de animacion que hara que se active EventExi y Transport
    }
}