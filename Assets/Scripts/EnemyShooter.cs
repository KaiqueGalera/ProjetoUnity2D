using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Referências")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Disparo")]
    public float timeBetweenShots = 2f;
    public float projectileSpeed = 5f;
    public bool isShooting = false;
    public bool shootRight = false;
    private Animator animator;
    public int health = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ShootingRoutine());
    }

    void Update()
    {
        animator.SetBool("IsShooting", isShooting);
    }

    IEnumerator ShootingRoutine()
    {
        while (true)
        {
            isShooting = true; // Ativa a animação de disparo
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = shootRight ? 1f : -1f;
                rb.velocity = new Vector2(direction * projectileSpeed, 0f);
            }
        }

        isShooting = false;
    }
    // public void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.CompareTag("Sword"))
    //     {
    //         //Animação de morte se quiser..
    //         Destroy(gameObject);
    //     }
    // }   
}
