using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeControl : MonoBehaviour
{
    private  GameController      _GameController;
    public   string              enemyName;

    [Header("Controle de dano dado")]
    // public   int                 flyMachineDmg;
    
    [Header("Controle de dano recebido")]
    public   int                 lifeMax;
    

    // Start is called before the first frame update
    void Start()
    {
        _GameController =    FindAnyObjectByType(typeof(GameController)) as GameController;
    }
}
