using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaseBird : MonoBehaviour
{
    private HitEffect _HitEffect;

    [Header("Movimentação")]
    public Transform[] points;
    public Transform enemy;
    public float speed = 5f;
    private int currentPointIndex = 1;
    public bool movingRight = false;

    [Header("Controle de Vida")]
    public int health = 30;
    public int currentHealth;

    [Header("Detecção de Jogador")]
    public float detectionRadius = 5f;
    private Vector3 targetPosition;
    private bool isChasingPlayer = false;

    [Header("Colisores")]
    public Collider2D detectionCollider; // o collider trigger padrão
    public Collider2D explosionCollider; // o collider ativado na explosão
    bool isExploding = false;


    void Start()
    {
        _HitEffect = GetComponent<HitEffect>();
        enemy.position = points[0].position;
        currentHealth = health;

        // Garante estado inicial dos colisores
        if (detectionCollider != null)
            detectionCollider.enabled = true;
        if (explosionCollider != null)
            explosionCollider.enabled = false;
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            Vector3 direction = targetPosition - transform.position;
            AdjustDirection(direction);

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(Explode());
            }

            return;
        }

        DetectPlayer();

        if (!isChasingPlayer)
        {
            float step = speed * Time.deltaTime;
            Transform nextPoint = points[currentPointIndex];
            
            Vector3 direction = nextPoint.position - transform.position;
            AdjustDirection(direction); // Corrigido: flipa antes de mover
            
            transform.position = Vector3.MoveTowards(transform.position, nextPoint.position, step);

            if (transform.position == nextPoint.position)
            {
                MoveToNextPoint();
            }
        }
    }

    

    void DetectPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRadius)
            {
                targetPosition = player.transform.position;
                isChasingPlayer = true;
            }
        }
    }

    IEnumerator Explode()
    {
        speed = 0f;

        // Ativa e desativa colisores corretamente
        DisableDetectionCollider();

        isExploding = true;
        
        // Se tiver animação, acione aqui
        GetComponent<Animator>().SetTrigger("Explode");

        yield return new WaitForSeconds(1f); // tempo da animação

        Destroy(gameObject); // remove o inimigo da cena
    }

    public void EnableDetectionCollider()
    {
        if (detectionCollider != null)
            detectionCollider.enabled = true;
    }

    public void DisableDetectionCollider()
    {
        if (detectionCollider != null)
            detectionCollider.enabled = false;
    }

    public void EnableExplosionCollider()
    {
        if (explosionCollider != null)
            explosionCollider.enabled = true;
    }

    public void DisableExplosionCollider()
    {
        if (explosionCollider != null)
            explosionCollider.enabled = false;
    }

    void AdjustDirection(Vector3 direction)
    {
        if (direction.x > 0 && !movingRight)
        {
            flip();
        }
        else if (direction.x < 0 && movingRight)
        {
            flip();
        }
    }

    void MoveToNextPoint()
    {
        currentPointIndex++;
        if (currentPointIndex >= points.Length)
            currentPointIndex = 0;
    }

    public void flip()
    {
        movingRight = !movingRight;
        Vector3 theScale = enemy.localScale;
        theScale.x *= -1;
        enemy.localScale = theScale;
    }

    public void TakeDmg(int dmg)
    {
        currentHealth -= dmg;
        _HitEffect.CallDmgHit();

        if (currentHealth <= 0)
        {
            speed = 0;
        }
    }

    // O trigger principal (detecção inicial)
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            LifeController _LifeController = col.GetComponent<LifeController>();
            if (_LifeController != null)
            {
                _LifeController.HeroDmgControl(15);
            }
        }

        // Aplica impulso se for a colisão da explosão
        if (isExploding && col.CompareTag("Player"))
        {
            HeroController hero = col.GetComponent<HeroController>();
            if (hero != null)
            {
                Vector2 dir = (col.transform.position - transform.position).normalized;
                hero.ApplyKnockback(dir, 15f); // força do empurrão
            }
        }    
}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
