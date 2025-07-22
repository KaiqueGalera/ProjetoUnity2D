using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float            blinkDuration = 3.0f;
    public Animator         fireAnimator;
    public Animator         explosionAnimator;
    public Collider2D       explosionCollider;
    public int              damageAmount = 10; // Quantidade de dano conferido ao jogador

    private bool            isExploding = false;
    private BarrelManager   barrelManager;
    private int             spawnIndex;
    private bool            isActive = false;

    void Start()
    {
        explosionCollider.enabled = false;
        barrelManager = FindObjectOfType<BarrelManager>();
        StartBlinking();
    }

    public void SetSpawnIndex(int index)
    {
        spawnIndex = index;
        Debug.Log($"Spawn index set to: {index}");
    }

    public void DelayedStartBlinking()
    {
        // Não inicia a rotina de explosão aqui
    }

    public void ActivateTrap()
    {
        isActive = true;
        StartBlinking();
    }

    // private IEnumerator StartBlinkingWithDelay()
    // {
    //     yield return new WaitForSeconds(2.0f); // Aguarda 2 segundos antes de iniciar a rotina de explosão
    //     StartBlinking();
    // }

    void StartBlinking()
    {
        if (isActive)
        {   
            fireAnimator.SetTrigger("StartFire");
            Invoke("Explode", blinkDuration);
        }
    }

    void Explode()
    {
        if (!isExploding)
        {
            isExploding = true;
            fireAnimator.SetTrigger("StopFire");
            explosionAnimator.SetTrigger("Explode");
        }
    }

    // Este método é chamado pelo evento da animação de explosão
    public void OnExplosionAnimationEnd()
    {
        EnableExplosionCollider();
        DisableExplosionCollider();
        Debug.Log($"Calling DestroyBarrel for spawn index: {spawnIndex}");
        barrelManager.DestroyBarrel(gameObject, spawnIndex);
    }

    void EnableExplosionCollider()
    {
        explosionCollider.enabled = true;
    }

    void DisableExplosionCollider()
    {
        explosionCollider.enabled = false;
    }

}
