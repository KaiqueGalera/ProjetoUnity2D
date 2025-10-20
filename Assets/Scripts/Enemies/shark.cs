using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    private HitEffect _HitEffect;
    public bool isDead = false;

    [Header("Movimentação")]
    public Transform[] points;       // Pontos de patrulha
    public Transform enemy;          // Referência ao inimigo (este objeto)
    public BoxCollider2D collider2DM;
    public float speed = 5f;         // Velocidade de movimento

    private int currentPointIndex = 1; 
    public bool isAttacking = false;
    public bool movingRight = false;

    [Header("Controle de Vida")]
    public int health = 30;
    public int currentHealth;

    void Start()
    {
        _HitEffect = GetComponent<HitEffect>();

        // Desativa o collider inicialmente
        // collider2DM.enabled = false;s

        // Inicializa posição do inimigo no primeiro ponto
        enemy.position = points[0].position;
        currentHealth = health;
    }

    void Update()
    {
        if (!isAttacking)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position, step);

            if (transform.position == points[currentPointIndex].position)
            {
                MoveToNextPoint();
                flip();
            }
        }
    }

    void MoveToNextPoint()
    {
        currentPointIndex++;

        if (currentPointIndex >= points.Length)
        {
            currentPointIndex = 0;
        }
    }

    public void flip()
    {
        movingRight = !movingRight;
        Vector3 theScale = enemy.localScale;
        theScale.x *= -1;
        enemy.localScale = theScale;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        _HitEffect.CallDmgHit();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Player":
                LifeController _LifeController = col.GetComponent<LifeController>();
                if (_LifeController != null)
                {
                    _LifeController.HeroDmgControl(25);
                }
                break;
        }
    }
    private void Die()
    {
        isDead = true;
        speed = 0;
        Destroy(gameObject, 1.0f); // Esperar a animação de morte antes de destruir o objeto
    }


}
