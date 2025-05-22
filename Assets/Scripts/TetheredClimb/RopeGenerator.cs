using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class RopeGenerator : MonoBehaviour
{
    public GameObject ropeSegmentPrefab;
    public Transform player1Attach;  
    public Transform player2Attach;  
    public int segmentCount = 20;

    private List<Transform> ropeSegments = new List<Transform>();
    private LineRenderer line;

    void Start()
    {
        transform.position = Vector3.zero;

        Debug.Log("Player1Attach position: " + player1Attach.position);
        Debug.Log("Player2Attach position: " + player2Attach.position);

        Vector2 start = player1Attach.position;
        Vector2 end = player2Attach.position;
        Vector2 direction = (end - start).normalized;
        float totalDistance = Vector2.Distance(start, end);
        float segmentLength = totalDistance / segmentCount;

        GameObject prevSegment = null;

        for (int i = 0; i < segmentCount; i++)
        {
            Vector2 pos = start + direction * segmentLength * i;
            GameObject segment = Instantiate(ropeSegmentPrefab, pos, Quaternion.identity, transform);
            Rigidbody2D rb = segment.GetComponent<Rigidbody2D>();
            HingeJoint2D joint = segment.GetComponent<HingeJoint2D>();

            if (i == 0)
            {
                joint.connectedBody = player1Attach.GetComponent<Rigidbody2D>();
            }
            else
            {
                joint.connectedBody = prevSegment.GetComponent<Rigidbody2D>();
            }

            ropeSegments.Add(segment.transform);
            prevSegment = segment;
        }

        if (prevSegment != null)
            prevSegment.GetComponent<HingeJoint2D>().connectedBody = player2Attach.GetComponent<Rigidbody2D>();

        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = ropeSegments.Count;
        line.textureMode = LineTextureMode.Tile;
        if (line.material != null)
            line.material.mainTextureScale = new Vector2(ropeSegments.Count, 1);

        RopeBetweenPlayers visual = GetComponent<RopeBetweenPlayers>();
        if (visual != null)
        {
            visual.ropeSegments = ropeSegments;
        }
    }

    void Update()
    {
        for (int i = 0; i < ropeSegments.Count; i++)
        {
            line.SetPosition(i, ropeSegments[i].position);
        }
    }
}
