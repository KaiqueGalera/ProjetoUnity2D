using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectil : MonoBehaviour
{
    private         TrapShootController     _TrapShootController;

    [Header("Config. Tiro")]
    public          float                   lifeTime;
    public          Vector2 direction =     Vector2.right;

    // Start is called before the first frame update
    void Start()
    {   
        _TrapShootController = FindAnyObjectByType(typeof(TrapShootController)) as TrapShootController;
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * _TrapShootController.speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
