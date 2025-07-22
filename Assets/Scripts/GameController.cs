using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour, IDataPersistence
{   
    public  HeroController      _HeroController;
    private AudioController     _AudioController;
    public  LifeController      _LifeController;
    public  PlatformController  _PlatformController;
    private IsNewGame           _IsNewGame;
    private DialogControl       _DialogControl;


    [Header("Avaliação ISO")]
    public  int                 score;

    
    [Header("Mouth Trap Config.")]
    public  float               mouthTimeTrap; // Tempo para armadilha ser acionada
    
    [Header("Boss Controllers")]
    public  int                 Attack1; 
    [Header("Hero Sword")]
    public int                  swordDmg;

    [Header("Hero Shoot")]
    public  int                 shootAmm;
    public  int                 shootDmg = 10;

    [Header("Hero invulnerability")]
    public  float               blinkTime; // Tempo piscando
    public  float               invulTime; // Tempo invulnerável 

    [Header("HUD GamePlay")]
    public  Image[]             lifeImg;
    public  Image[]             lifeImgM;
    public  Image[]             lifeImgH;
    public  Sprite              fullSr;
    public  Sprite              emptySr;
    // public  GameObject          normCollect;
    public  Image[]             normsFragments;
    public  GameObject          fragments;
    public  GameObject          norm;
    public  GameObject          openedNorm;
    public  Text                txtAmm;
    public  Text                txtDeaths;

    void Start()
    {
        _AudioController     = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _HeroController      = FindAnyObjectByType(typeof(HeroController)) as HeroController;
        _LifeController      = FindAnyObjectByType(typeof(LifeController)) as LifeController;
        _PlatformController  = FindAnyObjectByType(typeof(PlatformController)) as PlatformController;
        _IsNewGame           = FindAnyObjectByType(typeof(IsNewGame)) as IsNewGame;
        _DialogControl       = FindAnyObjectByType(typeof(DialogControl)) as DialogControl;
        
        norm.SetActive(false);
        openedNorm.SetActive(false);

        if(_AudioController != null)
        {
            _AudioController.FadeOut(); 
        }

        ApplyDifficultySettings();

        txtAmm.text = shootAmm.ToString();
        txtDeaths.text = _LifeController.heroDeaths.ToString();

        foreach (Image lives in lifeImg)
        {
            lives.gameObject.SetActive(true);
        }
    }

    public void AmmManager(int shootQtd)
    {
        shootAmm    +=  shootQtd;
        txtAmm.text =   shootAmm.ToString();
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadData(GameData data)
    {
        shootAmm = data.shootAmm;
        Attack1  = data.bossDmg;
    }

    public void SaveData(GameData data)
    {
        data.shootAmm = shootAmm;
        data.bossDmg  = Attack1;
    }

    void ApplyDifficultySettings()
    {
        if (_IsNewGame.isNewGame == true)
        {
            shootAmm                            = GameSettings.currentSettings.playerAmmShoot;
            _LifeController.lifes               = GameSettings.currentSettings.playerLives;
            if(_PlatformController != null)
            {
                _PlatformController.speedPlatform   = GameSettings.currentSettings.platformSpeed;
            }
            invulTime                           = GameSettings.currentSettings.playerInveTime;
            Attack1                             = GameSettings.currentSettings.bossAttack1;
            Debug.Log("Iniciando jogo pela primeira vez");
        }
    }
}
