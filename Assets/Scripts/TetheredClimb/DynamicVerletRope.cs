using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DynamicVerletRope : MonoBehaviour
{
    [Header("Puntos de Anclaje")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Configuración de la Cuerda")]
    public int segmentCount = 20;
    public float ropeLength = 2f;
    public int constraintIterations = 50;
    public Vector2 gravity = new Vector2(0, -9.81f);
    public float damping = 0.99f;

    private float segmentLength;
    private Vector3[] points;
    private Vector3[] prevPoints;
    private LineRenderer line;

    void Start()
    {
        segmentCount = Mathf.Max(2, segmentCount);
        segmentLength = ropeLength / (segmentCount - 1);

        points = new Vector3[segmentCount];
        prevPoints = new Vector3[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1);
            points[i] = Vector3.Lerp(startPoint.position, endPoint.position, t);
            prevPoints[i] = points[i];
        }

        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = segmentCount;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        points[0] = startPoint.position;
        points[segmentCount - 1] = endPoint.position;

        for (int i = 1; i < segmentCount - 1; i++)
        {
            Vector3 temp = points[i];
            points[i] = points[i] + (points[i] - prevPoints[i]) * damping + (Vector3)gravity * dt * dt;
            prevPoints[i] = temp;
        }

        for (int iteration = 0; iteration < constraintIterations; iteration++)
        {
            for (int i = 0; i < segmentCount - 1; i++)
            {
                Vector3 delta = points[i + 1] - points[i];
                float currentDist = delta.magnitude;
                float error = currentDist - segmentLength;
                Vector3 correction = (error / currentDist) * delta * 0.5f;

                if (i != 0)
                    points[i] += correction;
                if (i + 1 != segmentCount - 1)
                    points[i + 1] -= correction;
            }
            points[0] = startPoint.position;
            points[segmentCount - 1] = endPoint.position;
        }

        line.SetPositions(points);
    }
}
