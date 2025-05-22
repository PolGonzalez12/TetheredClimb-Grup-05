/**
 * HoverBold.cs
 * Añade este script al mismo GameObject que tenga el componente Button
 * (o cualquier Graphic con EventSystem).  Funciona con TextMeshProUGUI
 * y con el Text estándar.
 */
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class HoverBold : MonoBehaviour,
                         IPointerEnterHandler,
                         IPointerExitHandler
{
    // Referencia automática al texto del botón
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

        // Si no, busca el texto UI estándar
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
            tmpText.fontStyle |= FontStyles.Bold;       // añade negrita
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
