using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GerenciadorCreditosLoop : MonoBehaviour
{
    private AudioController _AudioController;
    public GameObject prefabResposta;         // Prefab com TextMeshProUGUI
    public RectTransform contentContainer;    // Content do Scroll View
    public float velocidadeRolagem = 30f;     // Velocidade da rolagem (pixels por segundo)

    private List<GameObject> respostasInstanciadas = new List<GameObject>();

    void Start()
    {
        _AudioController = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        foreach (var resposta in GerenciamentoResposta.Instance.respostas)
        {
            GameObject novaResposta = CriarItem(resposta.textoResposta, resposta.isCorreta);
            respostasInstanciadas.Add(novaResposta);
        }
    }

    void Update()
    {
        // Move o contentContainer para cima ao longo do tempo (como créditos)
        contentContainer.anchoredPosition += Vector2.up * velocidadeRolagem * Time.deltaTime;
    }

    public void MenuFases()
    {
        _AudioController.ChangeScene("Title 4", false, _AudioController.gamePlayMusic2);
    }

    GameObject CriarItem(string texto, bool correto)
    {
        GameObject novo = Instantiate(prefabResposta, contentContainer);

        var textoUI = novo.GetComponentInChildren<TextMeshProUGUI>();
        textoUI.text = texto;
        textoUI.color = correto ? Color.green : Color.red;
        textoUI.fontSize = 36;
        textoUI.alignment = TextAlignmentOptions.Center;

        return novo;
    }
}
