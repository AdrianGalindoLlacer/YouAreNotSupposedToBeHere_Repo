using Unity.VisualScripting;
using UnityEngine;

public class SOB_Hideout : MonoBehaviour
{
    [SerializeField] Transform Spot; // Variable que guarda el transform del empty donde el jugador se deslizara
    [SerializeField] GameObject Player; //Variable que guarda el player que se deslizara
    public bool Hiding =  false; //Variable que hara que el player deje de deslizarse
    [SerializeField] float Speed = 5f; //Velocidad a la que el player se esconde


    public void Update()
    {
        if (!Hiding) return;
        Player.transform.position = Vector3.MoveTowards(Player.transform.position, Spot.position, Speed * Time.deltaTime);

        if (Player.transform.position == Spot.position) Hiding = false;
    }
}
