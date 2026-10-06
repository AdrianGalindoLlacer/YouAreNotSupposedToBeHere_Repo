using UnityEngine;

public class PSC_PlayerController2 : MonoBehaviour
{
    #region General variables
    [Header("Movement")]
    [SerializeField] float Speed = 3f;
    [SerializeField] float SprintSpeed = 5f;
    [SerializeField] float CrouchSpeed = 5f;

    [Header("mechanics")]
    [SerializeField] Transform ShootPoint;
    [SerializeField] int Ammo;
    [SerializeField] int SpecialAmmo;
    [SerializeField] bool Sprinting;
    [SerializeField] bool Couching;

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
