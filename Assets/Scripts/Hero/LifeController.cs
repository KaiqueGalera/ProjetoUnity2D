using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour, IDataPersistence
{   
    private GameController      _GameController;
    private HealthBarUI         _HealthBarUI;
    private AudioController     _AudioController;
    private IsNewGame           _IsNewGame;
    private DataPersistenceManager  _dataPersistenceManager;

    [Header("Controle de vida")]
    private SpriteRenderer      heroSr;
    public  Color               invencibleColor;
    public  int                 currentHealth;
    public  int                 maxHealth = 100;
    public  int                 lifes;
    public  int                 heroDeaths = 0;
    public  int                 currentLifes;
    public  bool                isInvencible = false;

    // Start is called before the first frame update
    void Start()
    {
        _AudioController   =     FindAnyObjectByType(typeof(AudioController))  as AudioController;
        _GameController    =     FindAnyObjectByType(typeof(GameController))   as GameController;
        _HealthBarUI       =     FindAnyObjectByType(typeof(HealthBarUI))      as HealthBarUI;
        _IsNewGame         =     FindAnyObjectByType(typeof(IsNewGame))        as IsNewGame;
        _dataPersistenceManager = FindAnyObjectByType(typeof(DataPersistenceManager)) as DataPersistenceManager;

        heroSr           =     GetComponent<SpriteRenderer>();

        if (_IsNewGame.isNewGame == true)
        {
            Debug.Log("Inicilizando novos dados na vida !");
            currentHealth    =     maxHealth;
            currentLifes     =     lifes;
            _HealthBarUI.SetMaxHealth(maxHealth);
        }
        else
        {
            _HealthBarUI.SetHealth(currentHealth);
        }
        _dataPersistenceManager.isRestarting = false;
        UpdateLifes();
    }

    public void LoadData (GameData data)
    {
        currentLifes  = data.heroLifes;
        currentHealth = data.heroHealth;
        heroDeaths    = data.deaths;
    }

    public void SaveData (GameData data)
    {
        data.heroLifes  = this.currentLifes;
        data.heroHealth = this.currentHealth;
        data.deaths     = this.heroDeaths;
    }

    public void HeroDmgControl(int dmg) // Função para controlar o dano recebido pelo personagem. Recebe como parâmetro o dano específico do inimigo que desferiu o golpe
    {
        if (isInvencible == false)
        {
            StartCoroutine("BeInvencible");
            //_AudioController.sDmgHeroFx.PlayOneShot(_AudioController.dmgHeroFx);

            currentHealth -= dmg;
            _HealthBarUI.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentLifes --;
                //_AudioController.sRevive.PlayOneShot(_AudioController.revive);
                
                UpdateLifes();

                if (currentLifes <= 0)
                {
                    // _dataPersistenceManager.isRestarting = true;
                    _GameController.ChangeScene("GameOver");
                    
                }
                
                currentHealth = maxHealth;

                _HealthBarUI.SetHealth(currentHealth);
            }
        }

    }

    public IEnumerator BeInvencible() // método assincrono para ativar invencibilidade (conj. de caracteristicas) do personagem durante determinado tempo (invulTime)
    {   
        isInvencible = true;

        heroSr.color = invencibleColor; // Cor translúcida (0,0,0,30)
        
        StartCoroutine("BlinkInvencible");

        yield return new WaitForSeconds(_GameController.invulTime);

        heroSr.color = Color.white;

        isInvencible = false;

        heroSr.enabled = true;
        
        StopCoroutine("BlinkInvencible");
        
    }

    public IEnumerator BlinkInvencible() // método assincrono para piscar a imagem do personagem desativando e ativando sua sprite durante determinado tempo (blinkTime)
    {
        
        yield return new WaitForSeconds(_GameController.blinkTime);

        heroSr.enabled = !heroSr.enabled;
            
        StartCoroutine("BlinkInvencible");

    }

    public void UpdateLifes()
    {
        for (int i = 0; i < _GameController.lifeImg.Length; i++)
        {
            if (i < currentLifes)
            {
                _GameController.lifeImg[i].gameObject.SetActive(true);
                _GameController.lifeImg[i].sprite = _GameController.fullSr;
            }
            else 
            {
                _GameController.lifeImg[i].sprite = _GameController.emptySr;
            }
        }
    }

}
