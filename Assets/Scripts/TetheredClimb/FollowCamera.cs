using UnityEngine;


[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    [Header("Camera Target")]
    public GameObject target;                      
    public Vector3 target_offset = new(0, 0, -10);  
    [Range(1f, 20f)] public float camera_speed = 5f;

    [Header("Level Limits (manual)")]
    public float level_bottom = -10f;
    public float level_left = -40f;
    public float level_right = 40f;

    [Header("Opciones")]
    public bool clampHorizontal = true;            

    [Header("Dos jugadores (opcional)")]
    public Transform player1;
    public Transform player2;

    [Header("Calcular límites automáticamente (opcional)")]
    public bool autoDetectLimits = false;
    public Renderer levelBoundsRenderer;            

    Camera cam;
    Vector3 cur_vel;
    GameObject lock_target;
    float shake_timer, shake_intensity = 1f;
    Vector3 shake_vector;

    static FollowCamera _instance;

    void Awake()
    {
        _instance = this;
        cam = GetComponent<Camera>();

        if (autoDetectLimits && levelBoundsRenderer)
        {
            Bounds b = levelBoundsRenderer.bounds; 
            level_left = b.min.x;
            level_right = b.max.x;
            level_bottom = b.min.y;
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPos;

        if (player1 && player2)
        {
            desiredPos = (player1.position + player2.position) * 0.5f;
        }
        else
        {
            GameObject camTarget = lock_target ? lock_target : target;
            if (!camTarget) return;               
            desiredPos = camTarget.transform.position;
        }

        desiredPos += target_offset;

        float halfH = GetFrustumHeight() * 0.5f;
        float halfW = GetFrustumWidth() * 0.5f;

        desiredPos.y = Mathf.Max(level_bottom + halfH, desiredPos.y);

        if (clampHorizontal)
        {
            float minX = level_left + halfW;
            float maxX = level_right - halfW;

            if (minX < maxX)                    
                desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
            else                                
                desiredPos.x = (minX + maxX) * 0.5f;
        }

        transform.position = Vector3.SmoothDamp(transform.position,
                                                desiredPos,
                                                ref cur_vel,
                                                1f / camera_speed,
                                                Mathf.Infinity,
                                                Time.deltaTime);

        if (shake_timer > 0f)
        {
            shake_timer -= Time.deltaTime;
            shake_vector.Set(Mathf.Cos(shake_timer * Mathf.PI * 8f) * 0.02f,
                             Mathf.Sin(shake_timer * Mathf.PI * 7f) * 0.02f,
                             0f);
            transform.position += shake_vector * shake_intensity;
        }
    }

    public float GetFrustumHeight() =>
        cam.orthographic
            ? 2f * cam.orthographicSize
            : 2f * Mathf.Abs(transform.position.z) *
              Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

    public float GetFrustumWidth() => GetFrustumHeight() * cam.aspect;

    public void LockCameraOn(GameObject newTarget) => lock_target = newTarget;
    public void UnlockCamera() => lock_target = null;

    public void Shake(float intensity = 2f, float duration = 0.5f)
    {
        shake_intensity = intensity;
        shake_timer = duration;
    }

    public static FollowCamera Get() => _instance;
    public static Camera GetCam() => _instance ? _instance.cam : null;
    public static Camera GetCamera() => GetCam();   
}
