using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleController : MonoBehaviour
{   
    [Header("Menu Buttons")]
    [SerializeField] private Button         newGameBtn;
    [SerializeField] private Button         options;
    [SerializeField] private Button         leaveGame;
    // [SerializeField] private Button         continueBtn;

    private     GameSettings                _GameSettings;
    private     AudioController             _AudioController;
    private     IsDone                      _IsDone;
    private     DataPersistenceManager      _dataPersistenceManager;

    [Header("Interface - Opções")]
    [SerializeField] private GameObject painelMenuPrincipal;
    [SerializeField] private GameObject painelOpções;
    [SerializeField] private Slider firstSlider;

    [Header("Interface - Mudança de idioma")]
    private     LoadXMLFile                 _LoadXMLFile;
    public      TextMeshProUGUI             playText;
    public      TextMeshProUGUI             optionsText;
    public      TextMeshProUGUI             leaveGameText;


    // Start is called before the first frame update
    void Start()
    {
        _AudioController = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _IsDone = FindAnyObjectByType(typeof(IsDone)) as IsDone;
        _LoadXMLFile = FindAnyObjectByType(typeof(LoadXMLFile)) as LoadXMLFile;
        _GameSettings = FindAnyObjectByType(typeof(GameSettings)) as GameSettings;
        _dataPersistenceManager = FindAnyObjectByType(typeof(DataPersistenceManager)) as DataPersistenceManager;

        _AudioController.FadeOut();

        painelMenuPrincipal.SetActive(true);
        painelOpções.SetActive(false);
        
        playText.text = _LoadXMLFile.interface_titulo[0];
        optionsText.text = _LoadXMLFile.interface_titulo[1];
        leaveGameText.text = _LoadXMLFile.interface_titulo[2];
        



        // if (!DataPersistenceManager.Instance.HasGameData() || _dataPersistenceManager.isRestarting == true)
        // {
        //     continueBtn.interactable = false;
        // }

        // if (_dataPersistenceManager.isRestarting == true)
        // {
        //     continueBtn.interactable = false;
        // }

    }
    public void OnNewGameClicked()
    {
         if (_IsDone.isFadeDone == true)
         {
            DisabeAllBtnMenu();

            DataPersistenceManager.Instance.NewGame();

            _AudioController.ChangeScene("Titleteste2", false, _AudioController.gamePlayMusic);
         }
    }

    // public void OnLoadGameClicked()
    // {   
    //     if(_IsDone.isFadeDone == true)
    //     {   
    //         DisabeAllBtnMenu();

    //         // Salva o jogo sempre antes de carregar a cena 
    //         // DataPersistenceManager.Instance.SaveGame();

    //         // Carrega a próxima cena o que acarreta na chamada do OnSceneLoaded no DataPesistence o que por sua vez carrega dados previamente salvos
    //         _AudioController.ChangeScene("GamePlay", true, _AudioController.gamePlayMusic); 
    //     }
    // }

    public void OnOptionsSelect()
    {
        if (firstSlider != null)
        {
            // Pequeno delay para garantir que o UI está pronto
            StartCoroutine(SelectSliderAfterFrame());
        }
        painelMenuPrincipal.SetActive(false);
        painelOpções.SetActive(true);
    }

    private IEnumerator SelectSliderAfterFrame()
    {
        // Espera o final do frame atual
        yield return new WaitForEndOfFrame();
        
        // Limpa seleção atual
        EventSystem.current.SetSelectedGameObject(null);
        
        // Seleciona o slider
        EventSystem.current.SetSelectedGameObject(firstSlider.gameObject);
        firstSlider.Select();
    }
    public void OnOptionsExit()
    {
        painelOpções.SetActive(false);
        painelMenuPrincipal.SetActive(true);
    }

    public void OnSaveGameClicked()
    {
        DataPersistenceManager.Instance.SaveGame();
    }


    private void DisabeAllBtnMenu() // Método para desativar os botões do Menu principal, para evitar clique duplo ou clique em multiplos btns
    {
        newGameBtn.interactable   = false;
        leaveGame.interactable    = false;
        options.interactable      = false;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ChangeLanguage(string language)
    {
        PlayerPrefs.SetString("defaultLanguage", language);

        _LoadXMLFile.LoadXMLData();

        playText.text       = _LoadXMLFile.interface_titulo[0];
        optionsText.text    = _LoadXMLFile.interface_titulo[1];
        leaveGameText.text  = _LoadXMLFile.interface_titulo[2];
    }
}
