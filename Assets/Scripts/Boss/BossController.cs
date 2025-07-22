    using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private EqBoss          _EqBoss;
    private AudioController _AudioController;
    private GameController  _GameController;

    [Header("Colisores")]
    public Collider2D       attackPuch;
    public Collider2D       attackKick;
    public Collider2D       attackLaser;
    public Collider2D       mainCollider;
    public Collider2D       deadCollider;

    [Header("Animação")]
    public Animator         animator;

    [Header("Invulnerabilidade")]
    public SpriteRenderer   bossSr;
    public bool             isInvulnerable = false;
    public float            invulnerableDuration = 1.0f; // Duração da invulnerabilidade após sofrer dano
    public Color            invencibleColor;
    public float            blinkTime = 0.1f;

    [Header("Movimentação")]
    public bool             isBossActived;
    public Transform        player; // Referência ao jogador
    public float            moveSpeed = 2.0f; // Velocidade de movimento
    public bool             isFacingRight = true;

    [Header("Ataques")]
    public bool             isAttacking = false;

    [Header("Ataques Distância")]
    public float            rangeDistance; // Distância mínima para parar e atacar a distância
    public bool             isRangeAttack  = false;
    
    [Header("Morte")]
    public bool             isDead;

    [Header("Spawn enemies")]
    public GameObject       enemyPrefab; // Prefab dos inimigos que serão spawnados no attack4
    public Transform[]      spawnPoints; // Pontos onde os inimigos serão spawnados
    public float            stopDistance; // Distância mínima para parar e atacar
    public bool             isSummoned = false;

    [Header("Jump")]
    public float            jumpDistance;
    private Rigidbody2D     bossRb;
    public float            jumpForce;
    public bool             isJumping = false;

    [Header("Life")]
    public int              maxHealth = 100; // Vida máxima do boss
    private int             currentHealth;



    private void Start()
    {
        bossRb   = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        _EqBoss           = FindAnyObjectByType(typeof(EqBoss)) as EqBoss;
        _AudioController  = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _GameController   = FindAnyObjectByType(typeof(GameController)) as GameController;
        
        // Desativar colisores de ataque no início
        attackPuch.enabled   = false;
        attackKick.enabled   = false;
        attackLaser.enabled  = false;
        deadCollider.enabled = false;
        
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isDead)
        {
            if (isJumping || isAttacking)
                return;
            
            
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if ((distanceToPlayer > stopDistance) && isBossActived == true)
            {
                MoveTowardsPlayer();
            }
            else if (isBossActived == true)
            {
                StartCoroutine("PerformAttackSequence");
            }

            if (currentHealth <= maxHealth / 2 && !isSummoned)
            {
                isSummoned = true;
                // SpawnEnemies();
            }
        }    
    }
    
    private void OnDrawGizmosSelectd()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
    
    void MoveTowardsPlayer()
    {
        animator.SetBool("isWalking", true);
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        if ((player.position.x > transform.position.x && isFacingRight) || (player.position.x < transform.position.x && !isFacingRight))
        {
            Flip();
        }
    }
    
    IEnumerator PerformAttackSequence()
    {
        isAttacking = true;
        MoveTowardsPlayer(); // Ensure boss is facing the player correctly before attacking

        Attack1();
        yield return new WaitForSeconds(1f); // Adjust the delay as needed

        Flip();
        // JumpAway();
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(2f); // Adjust the delay as needed

        Attack2();
        yield return new WaitForSeconds(1f); // Adjust the delay as needed

        isAttacking = false;
    }    
    
    void Attack1()
    {
        animator.SetTrigger("Attack1");
    }

    void JumpAway()
    {
        isJumping = true;
        Vector2 jumpDirection = isFacingRight ? Vector2.left : Vector2.right;
        bossRb.velocity = new Vector2(0, 0); // Reset current velocity to prevent stacking with other forces
        bossRb.AddForce(jumpDirection * jumpDistance + Vector2.up * jumpForce, ForceMode2D.Impulse);
        Invoke(nameof(EndJump), 1f); // Adjust the delay as needed    
    }

    void EndJump()
    {
        isJumping = false;
    }

    void Attack2()
    {
        Flip();
        animator.SetTrigger("Attack2");
        // Add code for ranged attack
        // Invoke(nameof(ResumeMovement), 1f); // Adjust the delay as needed
    }
    

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine("BeInvencible");
        }
    }
    
    public IEnumerator BeInvencible() // método assincrono para ativar invencibilidade (conj. de caracteristicas) do personagem durante determinado tempo (invulTime)
    {   
        isInvulnerable = true;

        bossSr.color = invencibleColor; // Cor translúcida (0,0,0,30)
        
        StartCoroutine("BlinkInvencible");

        yield return new WaitForSeconds(invulnerableDuration);

        bossSr.color = Color.white;

        isInvulnerable = false;

        bossSr.enabled = true;

        StopCoroutine("BlinkInvencible");
        
    }

    public IEnumerator BlinkInvencible() // método assincrono para piscar a imagem do personagem desativando e ativando sua sprite durante determinado tempo (blinkTime)
    {
        
        yield return new WaitForSeconds(blinkTime);

        bossSr.enabled = !bossSr.enabled;
            
        StartCoroutine("BlinkInvencible");

    }

    void Flip()
    {
        if(!isDead)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
    
    private void Die()
    {
        // Destruir o boss ou realizar alguma animação de morte
        _EqBoss.isDialogOn = true;
        _EqBoss.StartConversation();
        isDead = true; // Boleana pra controlar movi e outros fatores de morte..
        animator.SetBool("isDead", true);
        _AudioController.isFaseEqDone = true;
        _AudioController.ChangeScene("ResultadosJogadorTeste 1", true, _AudioController.gamePlayMusic);
        
        // Destroy(gameObject);
    }
    
    
    // Métodos chamados pela animação via Animation Events
    public void EnableAttackPuch()
    {
        attackPuch.enabled = true;
    }

    public void DisableaPuch()
    {
        attackPuch.enabled = false;
    }

    public void EnableAttackKick()
    {
        attackKick.enabled = true;
    }

    public void DisableAttackKick()
    {
        attackKick.enabled = false;
    }

    public void EnableAttackLaser()
    {
        attackLaser.enabled = true;
    }

    public void DisableAttackLaser()
    {
        attackLaser.enabled = false;
    }

    public void EnableDeadCollider()
    {
        deadCollider.enabled = true;
    }

    public void DisableMainCollider()
    {
        mainCollider.enabled = false;
    }

    public void SpawnEnemies()
    {
        // animator.SetTrigger("isAttacking3"); Faltando

        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}