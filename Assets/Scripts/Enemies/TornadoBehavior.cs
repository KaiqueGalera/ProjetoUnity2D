using UnityEngine;

public class TornadoBehavior : MonoBehaviour
{
    [Header("Força de elevação do tornado")]
    public float liftForce = 10f;
    public float liftDuration = 0.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Certifique-se de que seu jogador tem a tag "Player"
        {
            HeroController hero = other.GetComponent<HeroController>();

            if (hero != null)
            {
                Vector2 direction = Vector2.up; // Direção para cima
                hero.ApplyKnockback(direction, liftForce);
            }
        }
    }
}
