using UnityEngine;
using UnityEngine.InputSystem;

public class PSC_PlayerShoot2 : MonoBehaviour
{
    public int Ammo = 3;

    private GameObject shootP;
    private Transform ShootPoint;
    private PSC_PlayerController2 P1;
    private GameObject Bullet; //almacena las balas
    private GameObject sound;
    private MAN_Player2SoundController soundC;
    private Animator anim; //almacena el animator del jugador


    private void Awake()
    {
        shootP = GameObject.Find("ShootPoint");
        sound = GameObject.Find("SoundManager");
        Bullet = GameObject.Find("SM_GunBullet");
        ShootPoint = shootP.transform.Find("ShootPoint");
        anim = GetComponent<Animator>();
        soundC = sound.GetComponent<MAN_Player2SoundController>();
    }
    void Shoot()
    {
        soundC.StopSound();
        soundC.playShoot();
        Instantiate(Bullet, ShootPoint.position, transform.rotation);
        Ammo--;
        //SOB_Bullet controller = Bullet.GetComponent<SOB_Bullet>();
        //controller.Move = true;
    }


    public void Onshot(InputAction.CallbackContext context)
    {
        if (P1.Block) return;
        if (!context.performed) return;

        if (Ammo <= 0)
        {
            P1.Block = true;
            anim.SetTrigger("NoAmmo");
        }
        else
        {
            P1.Block = true;
            anim.SetTrigger("Shoot");
        }
    }

}
