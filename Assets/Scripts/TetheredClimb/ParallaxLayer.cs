using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform; // La cámara principal
    public float parallaxFactor;      // Qué tan rápido se mueve esta capa (0 = fondo, 1 = se mueve igual que cámara)

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
