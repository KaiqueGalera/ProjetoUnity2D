using System.Collections;
using UnityEngine;

public class PlatformNvDesapear : MonoBehaviour
{
    public float fadeDuration = 2f;
    public float respawnTime = 5f;

    private SpriteRenderer[] spriteRenderers;
    private BoxCollider2D platformCollider;
    private bool isFading = false;

    void Start()
    {

        // spriteRenderers = GetComponents <SpriteRenderer>();
        platformCollider = GetComponent<BoxCollider2D>();
        
        // Acessa os irmãos pelo transform do pai (transform.parent)
        spriteRenderers = transform.parent.GetComponentsInChildren<SpriteRenderer>();        
        if (platformCollider == null)
            Debug.LogError("Collider da nuvem não encontrado!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFading) return;

        // Verifica se foi o pé do jogador que tocou
        if (collision.gameObject.tag == "Player")//collision.collider.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAndDisable());
        }
    }

    private IEnumerator FadeOutAndDisable()
    {
        isFading = true;

        float elapsedTime = 0f;
        Color[] initialColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            initialColors[i] = spriteRenderers[i].color;
        }

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = new Color(
                    initialColors[i].r,
                    initialColors[i].g,
                    initialColors[i].b,
                    alpha);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (var sr in spriteRenderers)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        }

        platformCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        StartCoroutine(Reappear());
    }

    private IEnumerator Reappear()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);

            foreach (var sr in spriteRenderers)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (var sr in spriteRenderers)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        }

        platformCollider.enabled = true;
        isFading = false;
    }
}
