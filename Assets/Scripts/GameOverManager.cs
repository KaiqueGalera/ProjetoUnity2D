using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    private AudioController         _audioController;
    private DataPersistenceManager  _dataPersistenceManager;
    public  Button                  menuBtn;

    // Start is called before the first frame update
    void Start()
    {

        _audioController        = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _dataPersistenceManager = FindAnyObjectByType(typeof(DataPersistenceManager)) as DataPersistenceManager;

        _dataPersistenceManager.DeleteGame();
        _dataPersistenceManager.AutoDestroy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {   
         menuBtn.interactable = false;
        _dataPersistenceManager.isRestarting = true;
        _audioController.ChangeScene("Title", true, _audioController.gamePlayMusic);
    }
}
