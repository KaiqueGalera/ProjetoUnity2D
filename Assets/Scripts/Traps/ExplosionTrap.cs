using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrap : MonoBehaviour
{
    [Header("SFX")]
    private AudioController _AudioController;
    public float            blinkDuration = 3.0f;
    public Animator         fireAnimator;
    public Animator         explosionAnimator;
    public Collider2D       explosionCollider;      

    private bool            isExploding = false;
    private BarrelRespawn   barrelManager;
    private int             groupIndex;
    private int             spawnIndex;
    private bool            isActive = false;

    void Start()
    {
        _AudioController = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        explosionCollider.enabled = false;
        barrelManager = FindObjectOfType<BarrelRespawn>();
    }

    public void SetGroupIndex(int index)
    {
        groupIndex = index;
    }

    public int GetGroupIndex()
    {
        return groupIndex;
    }

    public void SetSpawnIndex(int index)
    {
        spawnIndex = index;
    }

    public void SetBarrelManager(BarrelRespawn manager)
    {
        barrelManager = manager;
    }
    public void ActivateTrap()
    {
        isActive = true;
        StartBlinking();
    }

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
            _AudioController.sExplosion.PlayOneShot(_AudioController.explosion);
        }
    }

    void EnableExplosionCollider()
    {
        explosionCollider.enabled = true;
        Invoke("DisableExplosionCollider", 0.5f);
    }

    void DisableExplosionCollider()
    {
        explosionCollider.enabled = false;
    }
    void ToDestroy()
    {
        barrelManager.ToDestroyBarrel(gameObject, groupIndex, spawnIndex);
    }
    private void OnTriggerEnter(Collider other) // Parece que n era pra estar aqui... 
    {
        if (other.CompareTag("Activator") && !isActive)
        {
            print("Hello");
            BarrelRespawn.BarrelGroup group = barrelManager.barrelGroups[groupIndex];
            if (other == group.activatorCollider)
            {
                ActivateTrap();
            }
        }
    }
}