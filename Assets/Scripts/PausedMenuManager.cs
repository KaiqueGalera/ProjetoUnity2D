using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PausedMenuManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject     pausedMenuPanel;
    [SerializeField] private GameObject     optionsMenuPanel;
    
    [Header("Scripts para desativar no pause")]
    [SerializeField] private HeroController _heroController;
    
    [Header("Opções selecionadas")]
    [SerializeField] private GameObject     _mainMenuFirst;
    [SerializeField] private GameObject     _settingsMenuFirst;


    private DialogControl                   _dialogControl;
    private bool                            isPaused;
    void Start()
    {
        _dialogControl = FindAnyObjectByType(typeof(DialogControl)) as DialogControl;
        pausedMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.MenuOpenCloseInput)
        {
            if (!isPaused)
            {
                if (_dialogControl.panelDialog.activeSelf == false && _dialogControl.panelQuestions.activeSelf == false)
                {
                    Pause();
                }
            }
            else
            {
                Unpause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;

        Time.timeScale = 0f;

        _heroController.enabled = false;

        OpenMainMenu();

    }

    private void Unpause() // Função que despausa, desativando qualquer menu, e voltando o tempo do jogo
    {
        isPaused = false;

        Time.timeScale = 1.0f;
        
        _heroController.enabled = true;

        CloseAllMenus();
    }

    private void OpenMainMenu() // Ativa o menu principal e desativa o de opções
    {
        pausedMenuPanel.SetActive(true);
        
        optionsMenuPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    private void OpenSettingsMenu() // Ativa o menu de opções e desativa o principal
    {
        optionsMenuPanel.SetActive(true);
        
        pausedMenuPanel.SetActive(false);

    }

    private void CloseAllMenus() // Desativa todas os menus 
    {
        pausedMenuPanel.SetActive(false);

        optionsMenuPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

    }

    private void OpenSettingsMenuHandle()
    {
        pausedMenuPanel.SetActive(false);

        optionsMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);

    }

// Botões
    public  void OnSettingsPress()
    {
        OpenSettingsMenuHandle();
    }

    public  void OnSairPress()
    {
        Application.Quit();
    }
    public  void OnResumePress()
    {
        Unpause();
    }

    public  void OnSettingsBackPress()
    {
        OpenMainMenu();
    }
}
