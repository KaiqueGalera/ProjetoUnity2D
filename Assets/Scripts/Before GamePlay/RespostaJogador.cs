using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespostaJogador // Classe que representa as respostas dos jogadores (incluir mais intens para registro se necessário...)
{

    [SerializeField]
    public string textoResposta;
    public bool   isCorreta;

    public RespostaJogador(string texto, bool correta)
    {
        this.textoResposta = texto;
        this.isCorreta = correta;
    }
}
