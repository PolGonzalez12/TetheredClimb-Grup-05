using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform; // La c�mara principal
    public float parallaxFactor;      // Qu� tan r�pido se mueve esta capa (0 = fondo, 1 = se mueve igual que c�mara)

    private Vector3 previousCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - previousCameraPosition;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
        previousCameraPosition = cameraTransform.position;
    }
}
