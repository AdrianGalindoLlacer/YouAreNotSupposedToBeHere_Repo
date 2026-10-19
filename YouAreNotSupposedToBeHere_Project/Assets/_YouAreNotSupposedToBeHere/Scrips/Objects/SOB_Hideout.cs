using UnityEngine;

public class SOB_Hideout : MonoBehaviour
{
    [SerializeField] Transform Spot;
    [SerializeField] GameObject Player;
    public bool Hiding =  false;
    [SerializeField] float Speed = 5f;


    public void Update()
    {
        if (!Hiding) return;
        Player.transform.position = Vector3.MoveTowards(Player.transform.position, Spot.position, Speed * Time.deltaTime);

        if (Player.transform.position == Spot.position) Hiding = false;
    }
}
