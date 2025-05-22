using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingDotsTMP : MonoBehaviour
{
    private TMP_Text loadingText;
    private string baseText = "Cargando";
    private float dotInterval = 0.5f;

    void Start()
    {
        loadingText = GetComponent<TMP_Text>();
        StartCoroutine(AnimateDots());
    }

    IEnumerator AnimateDots()
    {
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount + 1) % 4;
            string dots = new string('.', dotCount);
            loadingText.text = baseText + dots;
            yield return new WaitForSeconds(dotInterval);
        }
    }
}
