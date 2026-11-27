using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PSC_PlayerShoot1 : MonoBehaviour
{
    [Header("Player")]
    public bool HoldRobot = true; //bool que guarda si el jugador lleva el robot
    public bool WalkRobot; //Bool que guarda si el robot esta en walk
    private PSC_PlayerController1 player;

    [Header("Robot")]
    [SerializeField] GameObject Robot; //el robot que el jugador lleva
    [SerializeField] private GameObject Spawn; //transform donde el robot aparecera
    private GameObject RobotThrow; //El robot que el jugador deja
    private SOB_Robot controller;

    [Header("Miscelaneos")]
    MAN_Player1SoundController soundC;
    private Animator anim;



    private void Awake()
    {
        RobotThrow = FindFirstObjectByType<SOB_Robot>().gameObject;
        player = GetComponent<PSC_PlayerController1>();
        anim = GetComponent<Animator>();
        soundC = FindFirstObjectByType<MAN_Player1SoundController>();
    }

    public void Start()
    {
        controller = RobotThrow.GetComponent<SOB_Robot>();
    }

    void Lay()
    {
        HoldRobot = false;
        if (!WalkRobot)
        {
            soundC.StopSound();
            soundC.playLay();
            Robot.SetActive(false);
            RobotThrow.transform.SetPositionAndRotation(Spawn.transform.position, transform.rotation);
            controller.soundenabled = true;
        }
        else
        {
            soundC.StopSound();
            soundC.playLay();
            Robot.SetActive(false);
            RobotThrow.transform.SetPositionAndRotation(Spawn.transform.position, transform.rotation);
            controller.Walk = true;
            controller.soundenabled = true;
        }
    }

    public void OnLay(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!HoldRobot)
        {
            anim.SetTrigger("NoRobot");
            player.Block = true;
        }
        else
        {
            anim.SetTrigger("Lay");
            player.Block = true;
        }
    }
}
