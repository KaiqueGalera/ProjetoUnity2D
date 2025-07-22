using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{   
    [Header("Menu Buttons")]
    [SerializeField] private Button         newGameBtn;
    [SerializeField] private Button         continueBtn;

    private     GameSettings                _GameSettings;
    private     AudioController             _AudioController;
    private     IsDone                      _IsDone;
    private     DataPersistenceManager      _dataPersistenceManager;

    [Header("Interface - Mudança de idioma")]
    private     LoadXMLFile                 _LoadXMLFile;
    public      Text                        playText;
    public      Text                        loadText;
    public      Text                        optionsText;


    // Start is called before the first frame update
    void Start()
    {
        _AudioController           = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _IsDone                    = FindAnyObjectByType(typeof(IsDone)) as IsDone;
        _LoadXMLFile               = FindAnyObjectByType(typeof(LoadXMLFile)) as LoadXMLFile;
        _GameSettings              = FindAnyObjectByType(typeof(GameSettings)) as GameSettings;
        _dataPersistenceManager    = FindAnyObjectByType(typeof(DataPersistenceManager)) as DataPersistenceManager;

        _AudioController.FadeOut();

        playText.text         = _LoadXMLFile.interface_titulo[0];
        optionsText.text      = _LoadXMLFile.interface_titulo[1];
        loadText.text         = _LoadXMLFile.interface_titulo[2];


        if (!DataPersistenceManager.Instance.HasGameData() || _dataPersistenceManager.isRestarting == true)
        {
            continueBtn.interactable = false;
        }

        if (_dataPersistenceManager.isRestarting == true)
        {
            continueBtn.interactable = false;
        }

    }
    public void OnNewGameClicked()
    {
         if (_IsDone.isFadeDone == true)
         {
            DisabeAllBtnMenu();

            DataPersistenceManager.Instance.NewGame();

            _AudioController.ChangeScene("Title 2", false, _AudioController.gamePlayMusic);
         }
    }

    public void OnLoadGameClicked()
    {   
        if(_IsDone.isFadeDone == true)
        {   
            DisabeAllBtnMenu();

            // Salva o jogo sempre antes de carregar a cena 
            // DataPersistenceManager.Instance.SaveGame();

            // Carrega a próxima cena o que acarreta na chamada do OnSceneLoaded no DataPesistence o que por sua vez carrega dados previamente salvos
            _AudioController.ChangeScene("GamePlay", true, _AudioController.gamePlayMusic); 
        }
    }

    public void OnSaveGameClicked()
    {
        DataPersistenceManager.Instance.SaveGame();
    }


    private void DisabeAllBtnMenu() // Método para desativar os botões do Menu principal, para evitar clique duplo ou clique em multiplos btns
    {
        newGameBtn.interactable     = false;
        continueBtn.interactable    = false;
    }


    public void ChangeLanguage(string language)
    {
        PlayerPrefs.SetString("defaultLanguage", language);

        _LoadXMLFile.LoadXMLData();

        playText.text         = _LoadXMLFile.interface_titulo[0];
        optionsText.text      = _LoadXMLFile.interface_titulo[1];
        loadText.text         = _LoadXMLFile.interface_titulo[2];
    }
}
