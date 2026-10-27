using UnityEngine;
using UnityEngine.UI;

public class MAN_Health : MonoBehaviour
{
    public float Health = 1f;
    [SerializeField] Image image;

    private void Update()
    {
        image.fillAmount = Health;
    }

}
