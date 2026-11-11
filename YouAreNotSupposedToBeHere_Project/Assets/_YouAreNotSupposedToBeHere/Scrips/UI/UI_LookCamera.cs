using UnityEngine;

public class UI_LookCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 direction = transform.position - cam.transform.position;
        direction.y = 0; // ignora la inclinación vertical
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}
