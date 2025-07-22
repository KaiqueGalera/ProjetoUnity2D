using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour
{
    private GameController  _GameController; 
    private EqBoss          _EqBoss;
    private BossController  _BossController;
    private DoorOpener      _DoorOpener;
    private FimFase2        _FimFase2;

    [Header("Btn selecionado")]
    public GameObject firstAnswer;

    [Header("Estado do jogo")]
    private GameStateManager _GameStateManager;

    [Header("Dialogo NCP")]
    private NPC  _NPC;
    public GameObject panelDialog;
    public GameObject gNPC;
    public Text npcName;
    public Text npcLines;
    public Queue<string> lines;
    public Image[] charImg;
    public Image[] charImgNorm;
    public Image[] charImgBoss;
    public GameObject[] imgCurrentNPC;
    public GameObject[] visualAlert;
    public Image currentCharImg;
    public Image currentNormImg;
    public bool isTyping;
    public float typingSpeed = 0.05f;

    [Header("Avaliação ISO")]
    public GameObject panelQuestions;
    public Image npcQuestionImg;
    public Image normQuestionImg;
    public GameObject[] currentQuestioner;
    public List<int> resultados_mix { get; private set; } = new List<int>();


    [Header("Sistema checagem de resposta")]
    public int range;
    public int? key;
    public bool isCorrectB1, isCorrectB2, isCorrectB3, isCorrectB4, isCorrectB5;
    public Text question;
    public Text questioner;
    public bool isCorrectNpc2, isCorrectNpc3, isCorrectNpc4, isCorrectNpc5, isCorrectNpc6;
    public Button[] btnAlternative;
    public Button[] btnAlternativeBoss;
    public Text[] textsBtn;
    public Text[] textsBtnBoss;
    public Text textAns;


    // Start is called before the first frame update
    void Start()
    {
        _GameStateManager = FindAnyObjectByType(typeof(GameStateManager)) as GameStateManager;
        _GameController = FindAnyObjectByType(typeof(GameController)) as GameController;
        _EqBoss = FindAnyObjectByType(typeof(EqBoss)) as EqBoss;
        _BossController = FindAnyObjectByType(typeof(BossController)) as BossController;
        _DoorOpener = FindAnyObjectByType(typeof(DoorOpener)) as DoorOpener;
        _NPC  = FindObjectOfType(typeof(NPC)) as NPC;
        _FimFase2  = FindObjectOfType(typeof(FimFase2)) as FimFase2;
        
        range = PlayerPrefs.GetInt("DificuldadeRange", 3);        
        panelDialog.SetActive(false); // O painel de dialogo começa fechado
        
        panelQuestions.SetActive(false); // O painel de perguntas começa fechado

        foreach (GameObject imgSpeaker in imgCurrentNPC)
        {
            imgSpeaker.SetActive(false);
        }
        foreach (GameObject imgQuestioner in currentQuestioner)
        {
            imgQuestioner.SetActive(false);
        }

        lines = new Queue<string>(); // Inicializa nova fila

        resultados_mix = SortearSemRepetir(range);

        Debug.Log("Números sorteados:");
        for (int i = 0; i < range; i++) 
        {
            Debug.Log(resultados_mix[i]);
        }
    }

    void Update()
    {
        // MUDAR PARA CHECAR SE OS FRAGMENTOS COM O ALPHA = 1;
        if (_NPC.isNPC2Q == true && _NPC.isNPC3Q == true && _NPC.isNPC4Q == true && _NPC.isNPC5Q == true && _NPC.isNPC6Q == true)
        {
            _GameController.fragments.SetActive(false);
            _GameController.norm.SetActive(true);
            if(_DoorOpener != null)
            {
                _DoorOpener.OpenDoor();
            }

            if(_FimFase2 != null)
            {
                _FimFase2.col.enabled = true;
            }
        }
    }

    public void NextLine()
    {
        StopCoroutine("TypeText"); // Certifica da Courotine estar encerrada para evitar sobreposição da "animação" letra a letra

        if (lines.Count == 0) // Caso as falas tenham acabado, chama função para encerrar conversa
        {
            if (gNPC == null)
            {
                _EqBoss.EndConversation();
            }
            else
            {
                gNPC.SendMessage("EndConversation", SendMessageOptions.DontRequireReceiver);
            }

            return;
        }

        string f = lines.Dequeue(); // Lê a proxima fala da fila

        StartCoroutine("TypeText", f); // Incia a Courotine que apresenta a informação na HUD, le tra a letra

    }
    public void recuperarResposta(int idAns)
    {
        int chave; 
        textAns = textsBtn[idAns];

        key   = GetKeyFromValue(gNPC.GetComponent<NPC>().AlternativesDictionary, textAns.text);
        
        if (key != null)
        {
            chave = key.Value;
            bool correta = chave == 1;
            GerenciamentoResposta.Instance.RegistrarResposta(textAns.text, correta); //ACESSA INSTANCIA P/ RGTRAR RSPOTA

            Debug.Log($"O valor '{textAns.text}' foi encontrado com a chave: {key}");
            SendAnswer(chave);
        }
        else
        {
            Debug.Log($"O valor '{textAns.text}' não foi encontrado no dicionário.");
        }

    }

    public int? GetKeyFromValue(Dictionary<int, string> dictionary, string value)
    {
        foreach (var pair in dictionary)
        {
            if (pair.Value == value)
            {
                return pair.Key;
            }
        }
        return null; // Retorna null se não encontrar
    }

    public void CheckKey(int expectedKey)
    {
        if (key == expectedKey)
        {
            Debug.Log("A chave corresponde ao valor esperado!");
        }
        else
        {
            Debug.Log("A chave não corresponde ao valor esperado ou não foi encontrada.");
        }
    }

    List<int> SortearSemRepetir(int range)
    {
        System.Random random = new System.Random(); // Para sorteios
        List<int> numeros = new List<int>();

        // Preenche a lista com números no intervalo [0, range - 1]
        for (int i = 0; i < range; i++)
        {
            numeros.Add(i);
        }

        List<int> resultado = new List<int>();

        // Sorteia os números sem repetição
        while (numeros.Count > 0)
        {
            int indice = random.Next(numeros.Count);
            resultado.Add(numeros[indice]);
            numeros.RemoveAt(indice); // Remove o número sorteado
        }

        return resultado;
    }
    
    public void SendAnswer(int idAnswer)
    {
        panelQuestions.SetActive(false);
        if (gNPC == null)
        {
            _EqBoss.isQuestioned = true;
            _EqBoss.EndConversation();
            
            if (idAnswer == 1)
            {
                if (_EqBoss.xmlName + _EqBoss.n == "Boss1")
                {
                    isCorrectB1 = true;
                    _EqBoss.n = 2;
                    _EqBoss.LoadXMLData(_EqBoss.n);
                    _BossController.TakeDamage(20);
                }
                else if (_EqBoss.xmlName + _EqBoss.n == "Boss2")
                {
                    isCorrectB2 = true;
                    _EqBoss.n = 3;
                    _EqBoss.LoadXMLData(_EqBoss.n);
                    _BossController.TakeDamage(20);
                }
            }
            else if (idAnswer == 2)
            {
                if (_EqBoss.xmlName + _EqBoss.n == "Boss3")
                {
                    isCorrectB3 = true;
                    _EqBoss.n = 4;
                    _EqBoss.LoadXMLData(_EqBoss.n);
                    _BossController.TakeDamage(20);
                }
            }
            else if (idAnswer == 0)
            {
                if (_EqBoss.xmlName + _EqBoss.n == "Boss4")
                {
                    isCorrectB4 = true;
                    _EqBoss.n = 5;
                    _EqBoss.LoadXMLData(_EqBoss.n);
                    _BossController.TakeDamage(20);
                }
                else if (_EqBoss.xmlName + _EqBoss.n == "Boss5")
                {
                    isCorrectB5 = true;
                    _EqBoss.n = 6;
                    _EqBoss.isQuestioned = true;
                    _BossController.TakeDamage(20);
                }
            }
            else
            {
                _EqBoss.isQuestioned = false;
            }
        }
        else
        {
            switch (gNPC.name)
            {
                case "NPC2":
                    gNPC.GetComponent<NPC>().isQuestioned = true;
                    _NPC.isNPC2Q = true;
                    _GameController.normsFragments[0].color = Color.white;

                    if (idAnswer == 1)
                    {
                        isCorrectNpc2 = true;
                    }
                    else
                    {
                        isCorrectNpc2 = false;
                    }
                    gNPC.GetComponent<NPC>().EndConversation();
                    break;

                case "NPC3":
                    gNPC.GetComponent<NPC>().isQuestioned = true;
                    _NPC.isNPC3Q = true;
                    _GameController.normsFragments[1].color = Color.white;

                    if (idAnswer == 1)
                    {
                        isCorrectNpc3 = true;
                    }
                    else
                    {
                        isCorrectNpc3 = false;
                    }

                    gNPC.GetComponent<NPC>().EndConversation();
                    break;

                case "NPC4":
                    gNPC.GetComponent<NPC>().isQuestioned = true;
                    _NPC.isNPC4Q = true;
                    _GameController.normsFragments[2].color = Color.white;

                    if (idAnswer == 1)
                    {
                        isCorrectNpc4 = true;
                    }
                    else
                    {
                        isCorrectNpc4 = false;
                    }

                    gNPC.GetComponent<NPC>().EndConversation();
                    break;

                case "NPC5":
                    gNPC.GetComponent<NPC>().isQuestioned = true;
                    _NPC.isNPC5Q = true;
                    _GameController.normsFragments[3].color = Color.white;

                    if (idAnswer == 1)
                    {
                        isCorrectNpc5 = true;
                    }
                    else
                    {
                        isCorrectNpc5 = false;
                    }

                    gNPC.GetComponent<NPC>().EndConversation();

                    break;

                case "NPC6":
                    gNPC.GetComponent<NPC>().isQuestioned = true;
                    _NPC.isNPC6Q = true;
                    _GameController.normsFragments[4].color = Color.white;

                    if (idAnswer == 1)
                    {
                        isCorrectNpc6 = true;
                    }
                    else
                    {
                        isCorrectNpc6 = false;
                    }

                    gNPC.GetComponent<NPC>().EndConversation();
                    break;
            }
        }
    }

    IEnumerator TypeText(string text) // Courotine para escrever letra a letra e 
    {
        isTyping = true;

        npcLines.text = "";

        foreach (char word in text.ToCharArray())
        {
            npcLines.text += word.ToString();
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;

    }

}
