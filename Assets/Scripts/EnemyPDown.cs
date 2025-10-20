using UnityEngine;

/// <summary>
/// Script genérico para instanciar projéteis (ex: pedra de gelo).
/// Usado em conjunto com evento do Animator.
/// </summary>
public class EnemyPDown : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    [Tooltip("Prefab do projétil que será instanciado.")]
    public GameObject projectilePrefab;

    [Tooltip("Ponto de onde o projétil será instanciado.")]
    public Transform spawnPoint;

    [Tooltip("Se verdadeiro, aplica apenas gravidade no Rigidbody2D do projétil.")]
    public bool useGravityOnly = true;

    [Tooltip("Velocidade inicial opcional do projétil (se não for apenas gravidade).")]
    public Vector2 initialVelocity = Vector2.zero;

    /// <summary>
    /// Chamado pelo Animator Event.
    /// </summary>
    public void SpawnProjectile()
    {
        if (projectilePrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("ProjectileSpawner sem prefab ou spawnPoint configurado!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (useGravityOnly)
            {
                rb.velocity = Vector2.zero;   // sem movimento inicial
                rb.gravityScale = 1f;         // cai pela gravidade
            }
            else
            {
                rb.gravityScale = 0f;         // não cai, movimento controlado
                rb.velocity = initialVelocity;
            }
        }
    }
}
