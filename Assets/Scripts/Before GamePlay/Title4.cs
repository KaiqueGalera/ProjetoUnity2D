using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Title4 : MonoBehaviour
{   
    [Header("Menu Buttons")]
    [SerializeField] private Button     equiBtn;
    [SerializeField] private Button     docBtn;
    [SerializeField] private Button     amostraBtn;

    private     GameSettings            _GameSettings;
    private     AudioController         _AudioController;
    private     IsDone                  _IsDone;

    [Header("Interface - Mudança de idioma")]
    private     LoadXMLFile             _LoadXMLFile;
    public      Text                    equip;
    public      Text                    doc;
    public      Text                    amostra; //Arrumar para ING



    // Start is called before the first frame update
    void Start()
    {
        _AudioController = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _IsDone          = FindAnyObjectByType(typeof(IsDone)) as IsDone;
        _LoadXMLFile     = FindAnyObjectByType(typeof(LoadXMLFile)) as LoadXMLFile;
        _GameSettings    = FindAnyObjectByType(typeof(GameSettings)) as GameSettings;

        _AudioController.FadeOut();

        equip.text        = _LoadXMLFile.interface_titulo[9];
        doc.text          = _LoadXMLFile.interface_titulo[10];

        if (_AudioController.isFaseEqDone){
            equiBtn.interactable = false;
        }
        if (_AudioController.isFaseDocDone){
            docBtn.interactable = false;
        }
        

    }

    public void SelecionarFase(string nivel)
    {
        switch (nivel)
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
            // case "Auditor":
            //     PlayerPrefs.SetInt("DificuldadeRange", 5); // Define range para 5
            //     DisabeAllBtn();
            //     // Carregar a próxima cena
            //     //_AudioController.ChangeScene("Title 2", false, _AudioController.gamePlayMusic);
            //     _AudioController.ChangeScene("GamePlay", true, _AudioController.gamePlayMusic);
            //     break;
        }

    }


    private void DisabeAllBtn() // Método para desativar os botões de escolha de dificuldade da jogabilidade, para evitar clique duplo ou clique em multiplos btns
    {
        equiBtn.interactable    = false;
        docBtn.interactable  = false;
        amostraBtn.interactable  = false;
    }
    
    public void ChangeLanguage(string language)
    {
        PlayerPrefs.SetString("defaultLanguage", language);

        _LoadXMLFile.LoadXMLData();

        equip.text         = _LoadXMLFile.interface_titulo[9];
        doc.text           = _LoadXMLFile.interface_titulo[10];
        // auditor.text       = _LoadXMLFile.interface_titulo[8];
    }
}
