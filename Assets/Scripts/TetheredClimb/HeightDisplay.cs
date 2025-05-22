using UnityEngine;
using TMPro;

public class HeightDisplay : MonoBehaviour
{
    public Transform referencePoint; 
    public Transform player;        
    public TextMeshProUGUI heightText;

    void Update()
    {
        float height = player.position.y - referencePoint.position.y;
        heightText.text = Mathf.Max(0, Mathf.RoundToInt(height)) + "m";
    }
}
