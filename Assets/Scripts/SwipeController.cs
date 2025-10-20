using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipeController : MonoBehaviour
{
    [Header("Configuração de Páginas")]
    [SerializeField] int maxPage;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagerRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    [Header("Texto da Fase")]
    [SerializeField] private TextMeshProUGUI faseText;

    private int currentPage;
    private Vector3 targetPos;

    private List<string> fasesNomes = new List<string>();

    private List<string> faseIDs = new List<string>() { "Equipamento", "Documento", "Amostra" };

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagerRect.localPosition;
    }

    public void SetFasesNomes(List<string> nomes)
    {
        fasesNomes = nomes;
        AtualizarTexto();
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    private void MovePage()
    {
        levelPagerRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
        AtualizarTexto();
    }

    private void AtualizarTexto()
    {
        if (faseText != null && fasesNomes.Count >= currentPage)
        {
            faseText.text = fasesNomes[currentPage - 1];
        }
    }

    public string GetFaseAtualID()
    {
        if (faseIDs != null && faseIDs.Count >= currentPage)
        {
            return faseIDs[currentPage - 1];
        }
        return string.Empty;
    }

    public string GetFaseAtualNome()
    {
        if (fasesNomes != null && fasesNomes.Count >= currentPage)
        {
            return fasesNomes[currentPage - 1];
        }
        return string.Empty;
    }
}
