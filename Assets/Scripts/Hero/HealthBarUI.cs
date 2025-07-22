using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public  Slider      sliderHealth;
    public  Gradient    gradient;
    public  Image       fill;

    public void SetMaxHealth(int health)
    {
        sliderHealth.maxValue = health;
        sliderHealth.value = health;

        fill.color = gradient.Evaluate(1.0f); // Define o valor da vida máxima como o valor máximo do gradiente (1) que significa verde
    }
        
        
    public void SetHealth(int health)
    {
        sliderHealth.value = health;

        fill.color = gradient.Evaluate(sliderHealth.normalizedValue); // Passa para o gradiente o valor normalizado do slider, ou seja, de 0 - 1;
    }
}
