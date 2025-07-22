using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title3 : MonoBehaviour
{   
    [Header("Menu Buttons")]
    [SerializeField] private Button     easyBtn;
    [SerializeField] private Button     normalBtn;
    [SerializeField] private Button     hardBtn;

    private     GameSettings            _GameSettings;
    private     AudioController         _AudioController;
    private     IsDone                  _IsDone;

    [Header("Interface - Mudança de idioma")]
    private     LoadXMLFile             _LoadXMLFile;
    public      Text                        easy;
    public      Text                        medium;
    public      Text                        hard;



    // Start is called before the first frame update
    void Start()
    {
        _AudioController = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _IsDone          = FindAnyObjectByType(typeof(IsDone)) as IsDone;
        _LoadXMLFile     = FindAnyObjectByType(typeof(LoadXMLFile)) as LoadXMLFile;
        _GameSettings    = FindAnyObjectByType(typeof(GameSettings)) as GameSettings;

        _AudioController.FadeOut();

        easy.text        = _LoadXMLFile.interface_titulo[6];
        medium.text      = _LoadXMLFile.interface_titulo[7];
        hard.text        = _LoadXMLFile.interface_titulo[8];

    }

    public void SetEasyDifficulty()
    {   
        DisabeAllDifficultyBtn();
        _GameSettings.SetDifficulty("Easy"); 
        _AudioController.ChangeScene("Title 4", false, _AudioController.gamePlayMusic);
        // _AudioController.ChangeScene("GamePlay" , true, _AudioController.gamePlayMusic); 
    }

    public void SetNormalDifficulty()
    {
        DisabeAllDifficultyBtn();
        _GameSettings.SetDifficulty("Normal");
        // _AudioController.ChangeScene("GamePlay", true, _AudioController.gamePlayMusic);
       _AudioController.ChangeScene("Title 4", false, _AudioController.gamePlayMusic);
 
        
    }

    public void SetHardDifficulty()
    {
        DisabeAllDifficultyBtn();
        _GameSettings.SetDifficulty("Hard");  
        // _AudioController.ChangeScene("GamePlay", true, _AudioController.gamePlayMusic); 
        _AudioController.ChangeScene("Title 4", false, _AudioController.gamePlayMusic);
    }


    private void DisabeAllDifficultyBtn() // Método para desativar os botões de escolha de dificuldade da jogabilidade, para evitar clique duplo ou clique em multiplos btns
    {
        easyBtn.interactable    = false;
        normalBtn.interactable  = false;
        hardBtn.interactable    = false;
    }
    
    public void ChangeLanguage(string language)
    {
        PlayerPrefs.SetString("defaultLanguage", language);

        _LoadXMLFile.LoadXMLData();

        easy.text         = _LoadXMLFile.interface_titulo[6];
        medium.text       = _LoadXMLFile.interface_titulo[7];
        hard.text         = _LoadXMLFile.interface_titulo[8];
    }
}
