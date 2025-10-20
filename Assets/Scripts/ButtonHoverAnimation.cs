using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverAnimation : MonoBehaviour,
    IPointerEnterHandler, 
    ISelectHandler
{
    public Animator animator;
    private AudioController _AudioController;
    public string animationName;
    private Button button;
    // public string idleAnimationName = "Idle"; 


    void Start()
    {
        _AudioController = FindFirstObjectByType<AudioController>();
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(PlayUIClick);
    }
    // Mouse hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverAnimation();
        PlayUIHover();
    }

    // public void OnPointerExit(PointerEventData eventData)
    // {

    // }
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayUIClick();
    }

    // Controle / teclado
    public void OnSelect(BaseEventData eventData)
    {
        PlayHoverAnimation();
        PlayUIHover();
    }

    // public void OnDeselect(BaseEventData eventData)
    // {

    // }

    private void PlayHoverAnimation()
    {
        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName);
        }
    }

    // private void PlayIdleAnimation()
    // {
    //     if (animator != null && !string.IsNullOrEmpty(idleAnimationName))
    //     {
    //         animator.Play(idleAnimationName);
    //     }
    // }

    private void PlayUIHover()
    {
        _AudioController.fx.PlayOneShot(_AudioController.houverSound);// PROCURAR AUDIO PROPÍCIO    
    }

    private void PlayUIClick()
    {
        _AudioController.fx.PlayOneShot(_AudioController.selectSound);// PROCURAR AUDIO PROPÍCIO    
    }

    void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(PlayUIClick);
    }
}
