using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
    [Header("Configurações de Dano")]
    public int damageAmount = 10;          // Dano causado ao herói
    public float damageInterval = 0f;      // Intervalo entre danos (0 = apenas uma vez)
    public bool destroyOnHit = false;      // Destroi o objeto após causar dano

    [Header("Som Opcional")]
    public AudioClip damageSound;          // Som do dano (ex: labareda, choque, etc.)
    public float soundVolume = 1f;
    public bool useExplosionChannel = false;  // Usa canal sExplosion do AudioController

    private AudioController _audioController;
    private float _nextDamageTime;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;

        _audioController = FindFirstObjectByType<AudioController>(); // Forma mais genérica (FindFirstObjectByType<t>)
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ApplyDamage(col);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (damageInterval > 0 && col.CompareTag("Player"))
        {
            if (Time.time >= _nextDamageTime)
            {
                ApplyDamage(col);
                _nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    private void ApplyDamage(Collider2D col)
    {
        // Herói controla o dano com HeroDmgControl()
        col.SendMessage("HeroDmgControl", damageAmount, SendMessageOptions.DontRequireReceiver);

        PlayDamageSound();

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    private void PlayDamageSound()
    {
        if (_audioController == null || damageSound == null)
            return;

        if (useExplosionChannel && _audioController.sExplosion != null)
        {
            _audioController.sExplosion.PlayOneShot(damageSound, soundVolume);
        }
        else if (_audioController.fx != null)
        {
            _audioController.fx.PlayOneShot(damageSound, soundVolume);
        }
    }
}
