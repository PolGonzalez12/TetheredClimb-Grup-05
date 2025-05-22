using UnityEngine;
using UnityEngine.UI;

public class FlechaGuiaUI : MonoBehaviour
{
    public Transform jugador; 
    public Sprite arrowUp, arrowDown, arrowLeft, arrowRight;
    public Image flechaImage; 

    void Start()
    {
        gameObject.SetActive(false); 
    }

    void Update()
    {
        if (!GameManager.instance.MisionActiva)
        {
            gameObject.SetActive(false);
            return;
        }

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        GameObject[] monedas = GameObject.FindGameObjectsWithTag("Moneda");
        if (monedas.Length == 0) return;

        GameObject objetivoCercano = monedas[0];
        float distanciaMin = Vector2.Distance(jugador.position, objetivoCercano.transform.position);

        foreach (GameObject moneda in monedas)
        {
            float distancia = Vector2.Distance(jugador.position, moneda.transform.position);
            if (distancia < distanciaMin)
            {
                distanciaMin = distancia;
                objetivoCercano = moneda;
            }
        }

        Vector2 direccion = (objetivoCercano.transform.position - jugador.position).normalized;

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            flechaImage.sprite = direccion.x > 0 ? arrowRight : arrowLeft;
        }
        else
        {
            flechaImage.sprite = direccion.y > 0 ? arrowUp : arrowDown;
        }
    }

}