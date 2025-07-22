using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    private Alarmobot[] alarmobots;

    void Start()
    {
        // Encontra todos os Alarmobots na cena
        alarmobots = FindObjectsOfType<Alarmobot>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Chama o método TriggerAlarm de todos os Alarmobots
            foreach (Alarmobot alarmobot in alarmobots)
            {
                alarmobot.TriggerAlarm();
            }
        }
    }
}