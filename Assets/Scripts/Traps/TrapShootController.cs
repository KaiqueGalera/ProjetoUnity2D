using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapShootController : MonoBehaviour
{   
    [Header("Projétil")]
    public          GameObject  shootPrefab;
    public          Transform   firePoint; // Local de partida do projétil
    public          float       fireRate; // Intervalo até o próximo disparo
    public          float       nextFireTime; // Tempo até o próximo disparo
    public          float       speed; // Velocidade do disparo

    public          bool        isShootLeft;
    public          bool        isTimeToShoot;
    private         bool        isFiredFirstShot = false;

    [Header("Animação")]
    private         Animator    trapShootAn;

    // Start is called before the first frame update
    void Start()
    {
        trapShootAn = GetComponent<Animator>();

        nextFireTime = Time.time;
        trapShootAn.SetTrigger("Shoot");
    }

    // Update is called once per frame
    void Update()
    {   
        if (Time.time >= nextFireTime && isTimeToShoot == true)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        else if (isFiredFirstShot == true)
        {
            trapShootAn.SetTrigger("Delay");
        }
    }

    void Shoot()
    {
        trapShootAn.SetTrigger("Shoot");

        Vector2 projectileDirection = Vector2.right;

        if (isShootLeft == true)
        {
            projectileDirection = Vector2.left;
        }
        GameObject projectile = Instantiate(shootPrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<ShootProjectil>().direction = projectileDirection;

        isFiredFirstShot = true;

    }

    void dalayShoot()
    {
        isTimeToShoot = true;
    }
}
