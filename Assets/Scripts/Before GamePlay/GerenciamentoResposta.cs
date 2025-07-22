using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciamentoResposta : MonoBehaviour
{
    public static GerenciamentoResposta Instance; // Declarando classe stática dizemos que não precisamos de um ref direta ao objeto
    public List<RespostaJogador> respostas = new List<RespostaJogador>(); // Lista que só aceita obj respostaJ, inicializada vazia
    
    void Awake()
    {
        if (Instance == null) // Checa se não existe instancia antes de criar
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas
        } else // Caso exista, destroy
        {
            Destroy(gameObject);
        }
    }

    public void RegistrarResposta(string texto, bool correta)
    {
        respostas.Add(new RespostaJogador(texto, correta));
    }

    public void LimparRespostas() // Caso queira reinicializar a lista durante jogo
    {
        respostas.Clear();
    }

}
