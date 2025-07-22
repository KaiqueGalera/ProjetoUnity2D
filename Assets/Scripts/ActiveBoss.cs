using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveBoss : MonoBehaviour
{
    private BossController _BossController;
    // Start is called before the first frame update
    void Start()
    {
        _BossController = FindAnyObjectByType(typeof(BossController)) as BossController;
    }

   void OnTriggerEnter2D(Collider2D other)
   {
    switch (other.tag)
    {
        case "Player":
            _BossController.isBossActived = true;
        break;
    }
   }
}
