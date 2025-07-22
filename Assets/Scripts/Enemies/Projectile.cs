using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configuração")]
    public float lifetime = 5f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifetime); // Autodestruição após um tempo
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Exemplo para aplicar dano, se desejar
        
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
       
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
