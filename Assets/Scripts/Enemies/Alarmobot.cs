using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Alarmobot : MonoBehaviour
{
    private GameController      _GameController;
    private HitEffect           _HitEffect;
    private Rigidbody2D         AlarmbotRb;
    private Animator            AlarmbotAn;
    private Transform           AlarmbotTransform;
    private BoxCollider2D       parentTriggerCol;
    private bool                isDead = false;
    private bool                isAlarmTriggered = false;
    private bool                isPlayerTriggered = false;
    public  float               speed;
    public  float               accelerationTime; // Tempo para atingir velocidade máxima
    public  int                 maxHealth = 100;
    public  int                 currentHealth;
    private bool                movingRight = true;
    private float               accelerationRate;
    public  float               currentSpeed;
    public  Transform           pointA; // Ponto inicial
    public  Transform           pointB; // Ponto final
    private Transform           targetPoint;

    void Start()
    {   
        _GameController     = FindAnyObjectByType(typeof(GameController)) as GameController;
        _HitEffect          = GetComponent<HitEffect>();
        AlarmbotRb          = GetComponent<Rigidbody2D>();
        AlarmbotAn          = GetComponent<Animator>();
        AlarmbotTransform   = transform; // Cache do transform
        AlarmbotTransform.position = pointA.position;
        currentHealth       = maxHealth;
        accelerationRate    = speed / accelerationTime;
        currentSpeed        = 0;
        targetPoint         = pointB; // Começa indo para o ponto B

        parentTriggerCol    = transform.parent.GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        if (isAlarmTriggered && !isDead)
        {
            Accelerate();
            Move();

            // Verifica se chegou no ponto de destino
            if (Vector2.Distance(AlarmbotTransform.position, targetPoint.position) < 0.1f)
            {
                Flip();
                StartCoroutine(IdleBeforeNextMove());
            }
        }
    }

    private void Move()
    {   
        int h = 0;
        // Vector2 direction = (targetPoint.position - AlarmbotTransform.position).normalized;
        if (targetPoint.position.x - AlarmbotTransform.position.x > 0)
        {
            h = 1;
        } else if (targetPoint.position.x - AlarmbotTransform.position.x < 0)
        {
            h = -1;
        }
        Vector2 direction = (targetPoint.position - AlarmbotTransform.position).normalized;
        AlarmbotRb.velocity = new Vector2(h * currentSpeed, AlarmbotRb.velocity.y);
    }

    public void Accelerate()
    {
        if (currentSpeed < speed)
        {
            currentSpeed += accelerationRate * Time.deltaTime;
        }
    }
    
    private void StopMovement()
    {
        currentSpeed = 0;
        AlarmbotRb.velocity = Vector2.zero; // Zera a velocidade do Rigidbody2D
    }

    public void Flip()
    {
        movingRight = !movingRight;
        Vector3 theScale = AlarmbotTransform.localScale;
        theScale.x *= -1;
        AlarmbotTransform.localScale = theScale;
        StopMovement(); // Resetar a velocidade ao mudar de direção

        // Alterna o ponto de destino
        targetPoint = (targetPoint == pointA) ? pointB : pointA;
    }

    public void TriggerAlarm()
    {
        if (!isPlayerTriggered)
        {
            AlarmbotAn.SetTrigger("Alert");
            
            isPlayerTriggered = true;
           
            StartCoroutine(StartRunningAfterAlert());
        }
    }

    public void AlarmTriggered()
    {
    }

    IEnumerator StartRunningAfterAlert()
    {
        yield return new WaitForSeconds(1.0f); // Duração da animação de alerta
        isAlarmTriggered = true;
        AlarmbotAn.SetTrigger("Run");
    }

    IEnumerator IdleBeforeNextMove()
    {
        isAlarmTriggered = false;
        AlarmbotAn.SetTrigger("Alert");
        yield return new WaitForSeconds(1.0f); // Duração da animação de alerta
        isAlarmTriggered = true;
        AlarmbotAn.SetTrigger("Run");
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

    private void Die()
    {
        isDead = true;
        AlarmbotAn.SetTrigger("Die");
        Destroy(gameObject, 1.0f); // Esperar a animação de morte antes de destruir o objeto
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                LifeController _LifeController = other.GetComponent<LifeController>();
                if (_LifeController != null)
                {
                    _LifeController.HeroDmgControl(15);
                }
                break;
            case "ShootH" :
                TakeDamage(_GameController.shootDmg);
                break;
            case "Sword" :
                TakeDamage(_GameController.swordDmg);
                break;
        }
        
    }
}