using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class RopeBetweenPlayers : MonoBehaviour
{
    public List<Transform> ropeSegments = new List<Transform>();
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = ropeSegments.Count;
        line.textureMode = LineTextureMode.Tile;
    }

    void Update()
    {
        for (int i = 0; i < ropeSegments.Count; i++)
        {
            line.SetPosition(i, ropeSegments[i].position);
        }
    }
}
