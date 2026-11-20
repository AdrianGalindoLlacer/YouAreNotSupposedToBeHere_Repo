using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PSC_PlayerShoot1 : MonoBehaviour
{
    public bool HoldRobot = true; //bool que guarda si el jugador lleva el robot
    public bool WalkRobot; //Bool que guarda si el robot esta en walk

    [SerializeField] GameObject Robot; //el robot que el jugador lleva
    [SerializeField] Transform Spawn; //transform donde el robot aparecera
    [SerializeField] GameObject RobotStand;
    [SerializeField] GameObject Origin;
    [SerializeField] GameObject RobotThrow; //El robot que el jugador deja
    [SerializeField] MAN_Player1SoundController soundC;
    SOB_Robot controller;
    Animator anim;
    PSC_PlayerController1 player;



    private void Awake()
    {
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
            RobotThrow.transform.SetPositionAndRotation(Spawn.position, transform.rotation);
            controller.soundenabled = true;
        }
        else
        {
            soundC.StopSound();
            soundC.playLay();
            Robot.SetActive(false);
            RobotThrow.transform.SetPositionAndRotation(Spawn.position, transform.rotation);
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
