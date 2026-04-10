using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //PATRON DE DISEÑO SINGLETON: UNA SOLA INSTANCIA DE NUESTRA CLASE

    public static UIManager Instance { get; private set; }

    public GameObject inventory;
    public GameObject pauseMenu;
    public GameObject statsPanel;
    public TMP_Text MoneyCountText;
    public TMP_Text MeatCountText;
    public TMP_Text WoodCountText;
    public TMP_Text HealthText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que el objeto se destruya al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta nueva instancia
        }
    }

    /// <summary>
    /// Cerrar o abrir inventario
    /// </summary>
    public void OpenOrCloseInventory ()
    {
        inventory.SetActive(!inventory.activeSelf);
    }

    /// <summary>
    /// Cerrar o abrir stats del jugador
    /// </summary>
    public void OpenOrCloseStatsPlayer()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    /// <summary>
    /// Actualizar contador de dinero en la UI
    /// </summary>
    /// <param name="countMoney"></param>
    public void UpdateMoney(int countMoney)
    {
        MoneyCountText.text = countMoney.ToString();
    }

    /// <summary>
    /// Actualizar contador de carne en la UI
    /// </summary>
    /// <param name="countMeat"></param>
    public void UpdateMeat(int countMeat)
    {
        MeatCountText.text = countMeat.ToString();
    }

    /// <summary>
    /// Actualizar maderea en la UI
    /// </summary>
    /// <param name="countWood"></param>
    public void UpdateWood(int countWood)
    {
        WoodCountText.text = countWood.ToString();
    }

    /// <summary>
    /// Actualiza la vida del jugador
    /// </summary>
    /// <param name="HealthValue"></param>
    public void UpdateHealth(int HealthValue)
    {
        HealthText.text = HealthValue.ToString();
    }

    /// <summary>
    /// Metodo pausar el juego
    /// </summary>
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Metodo de reanuar el juego
    /// </summary>
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
