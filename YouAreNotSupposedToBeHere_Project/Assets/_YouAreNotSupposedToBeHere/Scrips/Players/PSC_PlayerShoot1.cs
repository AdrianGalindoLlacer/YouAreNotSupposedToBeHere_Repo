using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PSC_PlayerShoot1 : MonoBehaviour
{
    [Header("Player")]
    public bool HoldRobot = true; //bool que guarda si el jugador lleva el robot
    public bool WalkRobot; //Bool que guarda si el robot esta en walk
    PSC_PlayerController1 player;

    [Header("Robot")]
    GameObject Robot; //el robot que el jugador lleva
    GameObject Spawn; //transform donde el robot aparecera
    GameObject RobotThrow; //El robot que el jugador deja
    SOB_Robot controller;

    [Header("Miscelaneos")]
    GameObject sound;
    MAN_Player1SoundController soundC;
    Animator anim;



    private void Awake()
    {
        Robot = GameObject.Find("SK_GwenRobotNoRig");
        Spawn = GameObject.Find("RobotSpawn");
        sound = GameObject.Find("SoundManager");
        RobotThrow = GameObject.Find("SK_Robot");
        soundC = sound.GetComponent<MAN_Player1SoundController>();
        player = GetComponent<PSC_PlayerController1>();
        anim = GetComponent<Animator>();
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
