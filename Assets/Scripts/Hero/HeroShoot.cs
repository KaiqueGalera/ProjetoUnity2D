using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroShoot : MonoBehaviour
{   
    private GameController  _GameController;

    void Start ()
    {
        _GameController = FindAnyObjectByType(typeof(GameController)) as GameController;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Alarmbot":
                // Lógica para aplicar dano ao Alarmbot
                Alarmobot _Alarmbot = other.GetComponent<Alarmobot>();
                if (_Alarmbot != null)
                {
                    _Alarmbot.TakeDamage(_GameController.shootDmg);
                }
                Destroy(gameObject);

                break;
            case "Enemy":
                // Lógica para aplicar dano a outros inimigos
                FlyMachine _FlyMachine = other.GetComponent<FlyMachine>();
                if (_FlyMachine != null)
                {
                    _FlyMachine.TakeDmg(_GameController.shootDmg);
                }
                Destroy(gameObject);

                break;
        }
    }
}