using UnityEngine;
public class CameraFocus : MonoBehaviour
{
    public Transform p1, p2;
    void LateUpdate()
    {
        if (p1 && p2)
            transform.position = (p1.position + p2.position) * 0.5f;
    }
}
