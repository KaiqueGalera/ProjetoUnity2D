using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Title2 : MonoBehaviour
{   
    [Header("Menu Buttons")]
    [SerializeField] private Button     analyBtn;
    [SerializeField] private Button     managerBtn;
    [SerializeField] private Button     auditorBtn;

    private     AudioController         _AudioController;

    [Header("Interface - Mudança de idioma")]
    private     LoadXMLFile             _LoadXMLFile;
    public      Text                        analy;
    public      Text                        manager;
    public      Text                        auditor;



    // Start is called before the first frame update
    void Start()
    {
        _AudioController = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _LoadXMLFile     = FindAnyObjectByType(typeof(LoadXMLFile)) as LoadXMLFile;

        _AudioController.FadeOut();

        analy.text        = _LoadXMLFile.interface_titulo[3];
        manager.text      = _LoadXMLFile.interface_titulo[4];
        auditor.text      = _LoadXMLFile.interface_titulo[5];

    }

    public void SelecionarDificuldade(string nivel)
    {
        switch (nivel)
        {
            case "Analista":
                PlayerPrefs.SetInt("DificuldadeRange", 3); // Define range para 3
                DisabeAllQDifficultyBtn();
                // Carregar a próxima cena
                _AudioController.ChangeScene("Title 3", false, _AudioController.gamePlayMusic);
                break;
            case "Gerente":
                PlayerPrefs.SetInt("DificuldadeRange", 4); // Define range para 4
                DisabeAllQDifficultyBtn();
                // Carregar a próxima cena
                _AudioController.ChangeScene("Title 3", false, _AudioController.gamePlayMusic);
                break;
            case "Auditor":
                PlayerPrefs.SetInt("DificuldadeRange", 5); // Define range para 5
                DisabeAllQDifficultyBtn();
                // Carregar a próxima cena
                _AudioController.ChangeScene("Title 3", false, _AudioController.gamePlayMusic);
                break;
        }

    }

    private void DisabeAllQDifficultyBtn() // Método para desativar os botões de escolha de dificuldade da jogabilidade, para evitar clique duplo ou clique em multiplos btns
    {
        analyBtn.interactable    = false;
        managerBtn.interactable  = false;
        auditorBtn.interactable  = false;
    }
    
    public void ChangeLanguage(string language)
    {
        PlayerPrefs.SetString("defaultLanguage", language);

        _LoadXMLFile.LoadXMLData();

        analy.text         = _LoadXMLFile.interface_titulo[3];
        manager.text       = _LoadXMLFile.interface_titulo[4];
        auditor.text       = _LoadXMLFile.interface_titulo[5];
    }
}
