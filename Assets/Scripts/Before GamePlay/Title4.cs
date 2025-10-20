using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Title4 : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button voltarBtn;
    [SerializeField] private Button sairBtn;
    [SerializeField] private Button aFabricaBtn;


    private GameSettings _GameSettings;
    private AudioController _AudioController;
    private IsDone _IsDone;

    [Header("Interface - Mudança de idioma")]
    private LoadXMLFile _LoadXMLFile;
    public TextMeshProUGUI voltar;
    public TextMeshProUGUI sair;
    public TextMeshProUGUI aFabrica; //Arrumar para ING
    public TextMeshProUGUI armazem; //Arrumar para ING
    public TextMeshProUGUI elementos; //Arrumar para ING



    // Start is called before the first frame update
    void Start()
    {
        _AudioController = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _IsDone = FindAnyObjectByType(typeof(IsDone)) as IsDone;
        _LoadXMLFile = FindAnyObjectByType(typeof(LoadXMLFile)) as LoadXMLFile;
        _GameSettings = FindAnyObjectByType(typeof(GameSettings)) as GameSettings;

        _AudioController.FadeOut();

        voltar.text = _LoadXMLFile.interface_titulo[17];
        sair.text = _LoadXMLFile.interface_titulo[2];
        aFabrica.text = _LoadXMLFile.interface_titulo[14];
        armazem.text = _LoadXMLFile.interface_titulo[15];
        elementos.text = _LoadXMLFile.interface_titulo[16];

        if (_AudioController.isFaseEqDone)
        {
            aFabricaBtn.interactable = false;
        }


        SwipeController swipe = FindAnyObjectByType<SwipeController>();
        if (swipe != null)
        {
            swipe.SetFasesNomes(new List<string>()
            {
                aFabrica.text,
                armazem.text,
                elementos.text
            });
        }
    }

    public void SelecionarFase()
    {
        SwipeController swipe = FindAnyObjectByType<SwipeController>();
        if (swipe == null) return;

        string faseAtualID = swipe.GetFaseAtualID();

        switch (faseAtualID)
        {
            case "Equipamento":
                DisabeAllBtn();
                _AudioController.ChangeScene("GamePlay", true, _AudioController.gamePlayMusic);
                break;
            case "Documento":
                DisabeAllBtn();
                _AudioController.ChangeScene("GamePlay 2", true, _AudioController.gamePlayMusic2);
                break;
            case "Amostra":
                DisabeAllBtn();
                _AudioController.ChangeScene("GamePlay 3", true, _AudioController.gamePlayMusic2);
                break;
        }
    }




    private void DisabeAllBtn() // Método para desativar os botões de escolha de dificuldade da jogabilidade, para evitar clique duplo ou clique em multiplos btns
    {
        voltarBtn.interactable = false;
        sairBtn.interactable = false;
    }

    public void ChangeLanguage(string language)
    {
        PlayerPrefs.SetString("defaultLanguage", language);

        _LoadXMLFile.LoadXMLData();

        voltar.text = _LoadXMLFile.interface_titulo[17];
        sair.text = _LoadXMLFile.interface_titulo[2];
        aFabrica.text = _LoadXMLFile.interface_titulo[14];
        armazem.text = _LoadXMLFile.interface_titulo[15];
        elementos.text = _LoadXMLFile.interface_titulo[16];

        SwipeController swipe = FindAnyObjectByType<SwipeController>();
        if (swipe != null)
        {
            swipe.SetFasesNomes(new List<string>()
            {
                aFabrica.text,
                armazem.text,
                elementos.text
            });
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void VoltarScene()
    {
        SceneManager.LoadScene("Titleteste3");
    }
}
