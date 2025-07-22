using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private float hitTime  = 0.25f;

    private SpriteRenderer          spriteRenderer;
    private Material                material;

    private Coroutine               hitChangeColorCoroutine; 
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Init();
    }

    // Update is called once per frame
    private void Init()
    {
        material = new Material(spriteRenderer.material);

        material = spriteRenderer.material;
    }

    public void CallDmgHit()
    {
        hitChangeColorCoroutine = StartCoroutine(HitColorChange());
    }

    private IEnumerator HitColorChange()
    {
        // Set the Color
        SetHitColor();

        // Lerp the hit amount
        float currentHitAmount = 0f;
        float elapsedTime      = 0f;

        while(elapsedTime < hitTime)
        {
            // iterate elapsedTime
            elapsedTime += Time.deltaTime;

            // lerp the hit amount
            currentHitAmount = Mathf.Lerp(1f, 0f, (elapsedTime / hitTime));
            SetHitAmount(currentHitAmount);

            yield return null;
        }
    }

    private void SetHitColor()
    {
        material.SetColor("_HitColor", hitColor);
    }

    private void SetHitAmount(float amount)
    {
        // set the hit amount
         material.SetFloat("_HitAmount", amount);
    }
}
