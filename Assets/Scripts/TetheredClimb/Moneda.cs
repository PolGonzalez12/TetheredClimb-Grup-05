using UnityEngine;

public class Moneda : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.I.PlaySFX(AudioManager.I.coinCollect);
            GameManager.instance.RecogerMoneda();
            Destroy(gameObject);
        }
    }
}
