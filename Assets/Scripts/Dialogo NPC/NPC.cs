using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.EventSystems;

public class NPC : MonoBehaviour
{   
    private DialogControl                   _DialogControl;
    private GameStateManager                _GameStateManager;

    [Header("Btn selecionado")]
    [SerializeField] private GameObject     _firstAnswer;

    [Header("XML")]
    public string                           xmlName; // Qual o arquivo de dialogo do NPC
    // public List<string>                              inicio_dialogo;
    public string                           defaultLanguage;

    [Header("Avaliação ISO")]
    public string                           question;
    public List<string>                     alternatives;
    private Dictionary<int, string>         alternativesDictionary = new Dictionary<int, string>();
    public Dictionary<int, string>          AlternativesDictionary => alternativesDictionary;
    private int                             indDic = 0; // Índice específico para cada NPC
    public  bool                            isNPC2Q = false, isNPC3Q = false, isNPC4Q = false, isNPC5Q = false, isNPC6Q = false;

    [Header("Controle de Dialogo")]
    public string                           npcName;
    public Dialog                           _Dialog;
    public List<Dialog>                     _Dialogues; // Array de conversas para que seja permitido trocar entre possíveis falas dentro do mesmo XML(e não ter somente uma fixa)
    public Dialog                           _LastDialogues; // classe que representa a última conversa que o NPC terá
    public bool                             isAllLinesDone;
    public bool                             isContinuous;
    public int                              idDialogue;
    public bool                             isQuestioned;
    public int                              first = 0; // Gambiarra para executar uma ação somente uma vez (podia ser bool)
 
    // Start is called before the first frame update
    void Start()
    {
        LoadXMLData();

        _DialogControl = FindObjectOfType(typeof(DialogControl)) as DialogControl;
        _GameStateManager =   FindObjectOfType(typeof(GameStateManager)) as GameStateManager;

    }

    public void NextCoversation() 
    {   
        if (isAllLinesDone == false)
        {
            idDialogue ++;

            if (idDialogue >= _Dialogues.Count) // Caso todas as falas foram executadas
            {
                isAllLinesDone = true; // Isso defini que ao terminar todas as falas iniciais, a conversa cairá para o fim conversa, onde será repetido somente esse xml

                _Dialog = _LastDialogues; // Dialogo recebe a classe que tem a última conversa

                return;
            }

            _Dialog.npcName = _Dialogues[idDialogue].npcName;
            _Dialog.lines   = _Dialogues[idDialogue].lines;
        } 
        else if (isAllLinesDone == true)
        {
            _Dialog = _LastDialogues;
        }
    }

    public void LoadXMLData()
    {
        if (PlayerPrefs.GetString("defaultLanguage") != "") // Caso a variável não exista (primeira vez entrando no jogo sem definir o idioma)
        {
            defaultLanguage = PlayerPrefs.GetString("defaultLanguage");            
        }

        // Limpa as falas depois da resposta
        idDialogue = 0;
        _Dialogues.Clear();

        // Limpa sistema de perguntas
        alternatives.Clear();
        question = null;
        // Limpa conversas finais
        _LastDialogues.lines.Clear();
        _LastDialogues.npcName = null;

        _LastDialogues = new Dialog();
        // As listas precisam ser apagadas antes de adicionar novas informações as mesmas
        // inicio_dialogo.Clear();
        
        TextAsset xmlData = (TextAsset)Resources.Load(defaultLanguage + "/" + xmlName); // Armazena o que está escrito dentro do arquivo passado na pasta Resources 

        XmlDocument xmlDocument = new XmlDocument(); // Instancia a classe XmlDocument (quase igual findObjectOfType, com a diferença que no caso do find a classe já está em execução no jogo)

        xmlDocument.LoadXml(xmlData.text); // Carrega o conteúdo do xml como text

        int i = 0;

        foreach (XmlNode node in xmlDocument["npc"].ChildNodes) // Para cada nó filho dentro do language do documento xml carregado
        {
            
            if (node.Name == "inicio_dialogo")
            {
                _Dialogues.Add(new Dialog());
    
                string nodeName = node.Attributes["name"].Value; // Armazene o valor dos nomes dos nós

                _Dialogues[i].npcName = nodeName;
                _Dialogues[i].lines   = new List<string>();

                foreach (XmlNode node2 in node["textos"].ChildNodes)
                {
                    _Dialogues[i].lines.Add(node2.InnerText);
                }

                i++;
            } 
            else if (node.Name == "pergunta_dialogo")
            {
                npcName = node.Attributes["name"].Value; // Armazene o valor dos nomes dos nós

                question = node["textos"].FirstChild.InnerText; // Armazena o conteúdo do primeiro filho do nó textos. Importante atentar-se que somente será lido o 1 filho

                // alternatives = new List<string>();

                // teste com dicionário
                // alternativesDictionary = new Dictionary<int, string>();


                foreach (XmlNode node2 in node["alternativas"].ChildNodes)
                {
                    //alternatives.Add(node2.InnerText);
                    alternativesDictionary.Add(indDic, node2.InnerText);
                    indDic ++;
                }
            }
            else if (node.Name == "fim_dialogo")
            {
                string nodeName = node.Attributes["name"].Value; // Armazene o valor dos nomes dos nós

                _LastDialogues.npcName = nodeName;
                _LastDialogues.lines = new List<string>();

                foreach (XmlNode node2 in node["textos"].ChildNodes)
                {
                    _LastDialogues.lines.Add(node2.InnerText);
                }
            }
        }

        if (isAllLinesDone == false)
        {
            _Dialog.npcName = _Dialogues[0].npcName;
            _Dialog.lines   = _Dialogues[0].lines;
        } 
        else
        {
            _Dialog = _LastDialogues;
        }
    }

    public void StartConversation() // Chamada sempre que novo dialogo é iniciado
    {   
        _GameStateManager.ChangeState(GameState.DialogState); // Muda o estado de jogo para o modo dialogo que congela o tempo
        _DialogControl.lines.Clear(); // Limpa falas anteriores

        _DialogControl.npcLines.text = ""; // Garante que o campo das falas do npc esteja vazio na exibição da HUD
        _DialogControl.npcName.text  = ""; // Garante que o campo do nome do npc esteja vazio na exibição na HUD

        _DialogControl.panelDialog.SetActive(true); // Ativa painel de dialogo

        foreach (string line in _Dialog.lines) // Le as falas do arquivo do NPC
        {
            _DialogControl.lines.Enqueue(line); // Coloca as conversas na fila
        }

        _DialogControl.npcName.text = _Dialog.npcName; // Atribui o nome do npc a variável npcName para apresentar na HUD
        
        if (_Dialog.npcName == "Requisito")
        {
            _Dialog.charImg = _DialogControl.charImgNorm[0].sprite;
        } 
        else if (_Dialog.npcName == "Maria")
        {
            _Dialog.charImg = _DialogControl.charImg[0].sprite;
        }
        else if (_Dialog.npcName == "Bruno")
        {
            _Dialog.charImg = _DialogControl.charImg[2].sprite;
        }
        else if (_Dialog.npcName == "Elisa") 
        {
            _Dialog.charImg = _DialogControl.charImg[3].sprite;
        }
        else if (_Dialog.npcName == "Joel")
        {
            _Dialog.charImg = _DialogControl.charImg[1].sprite;
        }
        
        _DialogControl.imgCurrentNPC[1].SetActive(true);
        _DialogControl.imgCurrentNPC[0].SetActive(false);

        _DialogControl.currentQuestioner[1].SetActive(true);
        _DialogControl.currentQuestioner[0].SetActive(false);

        _DialogControl.currentNormImg.sprite = _Dialog.charImg;
        _DialogControl.normQuestionImg.sprite = _Dialog.charImg;
        
        
        _DialogControl.NextLine(); // Chama a próxima fala invocando a função correspondente
    }

    public void EndConversation()
    {   
        _DialogControl.gNPC.GetComponent<NPC>().NextCoversation(); // Busca por um próximo dialogo no script do NPC

        if (isContinuous == true && isAllLinesDone == false) // Caso a conversa seja contínua, dá andamento a conversa
        {
             _DialogControl.gNPC.GetComponent<NPC>().StartConversation();
        } 
        else if (isQuestioned == true && isAllLinesDone == true && first == 0)
        {
            _DialogControl.gNPC.GetComponent<NPC>().first++;
             _DialogControl.gNPC.GetComponent<NPC>().StartConversation();
        }
        else
        {
            _DialogControl.panelDialog.SetActive(false);
            _GameStateManager.ChangeState(GameState.Playing); // Muda o estado de jogo para o gameplay que volta o tempo
            
            if (question != null && isQuestioned == false)
            {
                EventSystem.current.SetSelectedGameObject(_DialogControl.firstAnswer);

                _GameStateManager.ChangeState(GameState.DialogState); // Muda o estado de jogo para o Dialogo que para o tempo
                
                _DialogControl.panelQuestions.SetActive(true);
                _DialogControl.questioner.text = npcName;
                _DialogControl.question.text   = question;

                for (int e = 0; e < _DialogControl.range; e++)
                {
                    _DialogControl.btnAlternative[_DialogControl.resultados_mix[e]].gameObject.SetActive(true);
                    _DialogControl.textsBtn[e].text = alternativesDictionary[_DialogControl.resultados_mix[e]];
                    Debug.Log(alternativesDictionary[_DialogControl.resultados_mix[e]]);    
                }
            }
        }
    }

}
