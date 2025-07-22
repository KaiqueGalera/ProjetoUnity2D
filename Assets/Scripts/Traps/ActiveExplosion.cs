using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveExplosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Encontrar todos os barris na cena e ativar suas armadilhas
            ExplosionTrap[] barrels = FindObjectsOfType<ExplosionTrap>();
            foreach (ExplosionTrap barrel in barrels)
            {
                barrel.ActivateTrap();
            }
        }
    }
}
