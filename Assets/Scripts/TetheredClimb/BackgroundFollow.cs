using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Parallax")]
    [Tooltip("0 = fondo fijo · 1 = se mueve igual que los jugadores")]
    [Range(0f, 1f)] public float followFactor = 0.1f;  
    public bool followY = false;                       

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        Vector3 midpoint = (player1.position + player2.position) * 0.5f;

        Vector3 targetPos = startPos + new Vector3(
            midpoint.x * followFactor,
            followY ? midpoint.y * followFactor : 0,
            0);

        transform.position = targetPos;
    }
}
