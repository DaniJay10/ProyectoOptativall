using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject buttonsPanel;
    public GameObject mainMenuTitle;
    public GameObject SkinSelected;
    public GameObject SkinSelector;

    public TMP_Dropdown qualityDropdown;

    public GameObject mainPlayerSkin;
    public Sprite[] spritesSkins;
    public string[] skinNames = { "Purple", "Blue", "Red", "Yellow" };


    private void Start()
    {
        PlayerPrefs.SetString("MainPlayerSkin", skinNames[0]);


        int savedQuitality = PlayerPrefs.GetInt("QualityLevel", -1);

        if(savedQuitality == -1)
        {
            savedQuitality = QualitySettings.GetQualityLevel();
        }
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
        qualityDropdown.value = savedQuitality;

        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Abrir menu de opciones
    /// </summary>
    public void OpenOptions()
    {
        //desactivar
        buttonsPanel.SetActive(false);
        mainMenuTitle.SetActive(false);
        SkinSelected.SetActive(false);
        SkinSelector.SetActive(false);

        //activas
        optionsPanel.SetActive(true);
    }
    
    /// <summary>
    /// Cerrar menu de opciones
    /// </summary>
    public void CloseOptions()
    {
        //activar
        buttonsPanel.SetActive(true);
        mainMenuTitle.SetActive(true);
        SkinSelected.SetActive(true);
        SkinSelector.SetActive(true);

        //desactivar
        optionsPanel.SetActive(false);
    }

    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.GetInt("QualityLevel", index);
    }

    /// <summary>
    /// Funcion para seleccionar skin
    /// </summary>
    /// <param name="index"></param>
    public void SelectSkin(int index)
    {
        mainPlayerSkin.GetComponent<Image>().sprite = spritesSkins[index];
        PlayerPrefs.SetString("MainPlayerSkin", skinNames[index]);
    }
}
