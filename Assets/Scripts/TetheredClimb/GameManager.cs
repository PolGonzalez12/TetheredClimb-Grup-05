using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public bool MisionActiva = false;

    [Header("Monedas")]
    public int monedasRecolectadas = 0;
    public int totalMonedas = 10;

    [Header("Referencias UI")]
    public GameObject coinContainer;
    public TextMeshProUGUI textoCoinCount;
    public TextMeshProUGUI textoMision;

    [Header("Otros")]
    public GameObject flechaGuiaUI;

    const float delayBeforeScene = 1.5f;  

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        coinContainer.SetActive(false);
        textoMision.text = "";
        ActualizarConteoMonedas();
        if (flechaGuiaUI) flechaGuiaUI.SetActive(false);
    }

    public void StartMission()
    {
        Debug.Log("¡Misión iniciada!");
        MisionActiva = true;

        textoMision.text = "Misión: consigue 10 monedas";
        coinContainer.SetActive(true);
        ActualizarConteoMonedas();
        if (flechaGuiaUI) flechaGuiaUI.SetActive(true);
    }

    public void RecogerMoneda()
    {
        if (!MisionActiva) return;       

        monedasRecolectadas++;
        ActualizarConteoMonedas();

        if (monedasRecolectadas >= totalMonedas)
            MisionCompletada();
    }

    void MisionCompletada()
    {
        MisionActiva = false;

        textoMision.text = "¡Misión completada!\nRegresa al Señor Seta.";
        Debug.Log("¡Todas las monedas recogidas!");
        if (flechaGuiaUI) flechaGuiaUI.SetActive(false);

        Invoke(nameof(CargarEscenaFinal), delayBeforeScene);
    }

    void CargarEscenaFinal()
    {
        SceneManager.LoadScene("FinalScene");
    }

    void ActualizarConteoMonedas()
    {
        textoCoinCount.text = $"{monedasRecolectadas} / {totalMonedas}";
    }
}
