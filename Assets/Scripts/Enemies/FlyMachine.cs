using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FlyMachine : MonoBehaviour
{
    
    private GameController   _GC;
    private HitEffect        _HitEffect;
    public  Transform[]      points; // Array para armazenar os três pontos
    public  Transform        enemy; // Array para armazenar os três pontos
    public  BoxCollider2D    collider2DM;    
    public  float            speed = 5f; // Velocidade de movimento
    public  float            attackDuration = 1f; // Duração do ataque

    private int              currentPointIndex = 1; // Índice do ponto atual
    public  bool             isAttacking = false; // Flag para verificar se está atacando
    public  Animator         animator; // Referência para o Animator
    
    [Header("Controle de vida FlyMachine")]
    public  int              health = 30;
    public  int              currentHealth;



    void Start()
    {
        _GC         = FindAnyObjectByType(typeof(GameController)) as GameController;
        _HitEffect  = GetComponent<HitEffect>();

        collider2DM.GetComponent<BoxCollider2D>().enabled = false;
        
        enemy.position = points[0].position;
        MoveToNextPoint();

        currentHealth = health;
    }

    void Update()
    {
        if (!isAttacking)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position, step);

            if (enemy.position == points[currentPointIndex].position)
            {
                StartCoroutine("Attack");
            }
        }
    }

    void MoveToNextPoint()
    {
        if(enemy.position == points[currentPointIndex].position)
        {
            currentPointIndex++;
                
            if(currentPointIndex == points.Length)
            {
                currentPointIndex = 0;
            }
        }
    }
     
     IEnumerator Attack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        MoveToNextPoint();
    }
    void  EnableCo2D() // Função para habilitar o colisor da trap *É evocada na própria unity pelo animator
    {
        collider2DM.enabled = true;
    }
    
    void DisableCo2D() // Função para desabilitar o colisor da trap *É evocada na própria unity pelo animator
    {
        collider2DM.enabled = false;
    }
    void FlyDestroy() // Função para desabilitar o colisor da trap *É evocada na própria unity pelo animator
    {
        Destroy(this.gameObject);
    }

    public void TakeDmg(int dmg)
    {
        currentHealth -= dmg;

        _HitEffect.CallDmgHit();

        if (currentHealth <= 0)
        {
            speed = 0;
            animator.SetTrigger("Die");        
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
                    _LifeController.HeroDmgControl(15);
                }
                break;
        }
    }
    void LateUpdate()
    {
        animator.SetBool("isAttacking", isAttacking);
    }
}