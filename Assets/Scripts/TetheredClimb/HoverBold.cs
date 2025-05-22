/**
 * HoverBold.cs
 * A�ade este script al mismo GameObject que tenga el componente Button
 * (o cualquier Graphic con EventSystem).  Funciona con TextMeshProUGUI
 * y con el Text est�ndar.
 */
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class HoverBold : MonoBehaviour,
                         IPointerEnterHandler,
                         IPointerExitHandler
{
    // Referencia autom�tica al texto del bot�n
    TMP_Text tmpText;
    UnityEngine.UI.Text uiText;

    // Graba el estilo original para restaurarlo
    TMPro.FontStyles originalTMPStyle;
    FontStyle originalUIStyle;

    void Awake()
    {
        // Busca TextMeshPro primero
        tmpText = GetComponentInChildren<TMP_Text>();
        if (tmpText)
            originalTMPStyle = tmpText.fontStyle;

        // Si no, busca el texto UI est�ndar
        if (!tmpText)
        {
            uiText = GetComponentInChildren<UnityEngine.UI.Text>();
            if (uiText)
                originalUIStyle = uiText.fontStyle;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tmpText)
            tmpText.fontStyle |= FontStyles.Bold;       // a�ade negrita
        else if (uiText)
            uiText.fontStyle = FontStyle.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tmpText)
            tmpText.fontStyle = originalTMPStyle;       // restaura
        else if (uiText)
            uiText.fontStyle = originalUIStyle;
    }
}
