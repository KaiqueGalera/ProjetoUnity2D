using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ResumoRespostasUI : MonoBehaviour
{
    public GameObject respostaItemPrefab;
    public Transform contentContainer;

    public Color corCorreta = Color.green;
    public Color corIncorreta = Color.red;

    void Start()
    {
        List<RespostaJogador> respostas = GerenciamentoResposta.Instance.respostas;

        foreach (var resposta in respostas)
        {
            GameObject item = Instantiate(respostaItemPrefab, contentContainer);

            TextMeshProUGUI textoResposta = item.transform.Find("Texto").GetComponent<TextMeshProUGUI>();

            textoResposta.text = resposta.textoResposta;
            textoResposta.color = resposta.isCorreta ? Color.green : Color.red;
        }
    }
}
