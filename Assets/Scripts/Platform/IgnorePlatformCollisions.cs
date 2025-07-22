using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlatformCollisions : MonoBehaviour
{
    private Collider2D enemyCollider;

    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            Physics2D.IgnoreCollision(enemyCollider, collision.collider);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platform"))
        {

            Physics2D.IgnoreCollision(enemyCollider, other);
        }
    }
}