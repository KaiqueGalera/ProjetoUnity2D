using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Diagnostics;

public class LoadXMLFile : MonoBehaviour
{   
    public string           xmlFile;
    public string           defaultLanguage; // Já define como padrão pt-br

    public List<string>     interface_titulo;
    public List<string>     interface_conversa;
    public List<string>     interface_dialogo;



    // Start is called before the first frame update
    void Awake()
    {
        LoadXMLData();
    }

    public void LoadXMLData()
    {
        if (PlayerPrefs.GetString("defaultLanguage") != "") // Caso a variável não exista (primeira vez entrando no jogo sem definir o idioma)
        {
            defaultLanguage = PlayerPrefs.GetString("defaultLanguage");            
        }

        // As listas precisam ser apagadas antes de adicionar novas informações as mesmas
        interface_conversa.Clear();
        interface_titulo.Clear();
        interface_dialogo.Clear();
        
        TextAsset xmlData = (TextAsset)Resources.Load(defaultLanguage + "/" + xmlFile); // Armazena o que está escrito dentro do arquivo passado na pasta Resources 

        XmlDocument xmlDocument = new XmlDocument(); // Instancia a classe XmlDocument (quase igual findObjectOfType, com a diferença que no caso do find a classe já está em execução no jogo)

        xmlDocument.LoadXml(xmlData.text); // Carrega o conteúdo do xml como text

        foreach (XmlNode node in xmlDocument["language"].ChildNodes) // Para cada nó filho dentro do language do documento xml carregado
        {
            string nodeName = node.Attributes["name"].Value; // Armazene o valor dos nomes dos nós

            // print(nodeName); // Escreva os nomes armazenados

            foreach (XmlNode node2 in node["textos"].ChildNodes)
            {
                switch(nodeName)
                {
                case "interface_titulo":
                
                    interface_titulo.Add(node2.InnerText); // Armazena os textos das tags dentro da lista interface_titulo
                    break;

                case "interface_conversa": // Escreva o conteúdo dentro dos nós filhos de textos (quando nó anterior é : Interce_conversa)

                    interface_conversa.Add(node2.InnerText); // Armazena os textos das tags dentro da lista interface_conversa
                    break;

                case "dialogo": // Escreva o conteúdo dentro dos nós filhos de textos (quando nó anterior é : Interce_conversa)

                    interface_dialogo.Add(node2.InnerText); // Armazena os textos das tags dentro da lista interface_conversa
                    break;
                }
            }
        }
    }
}
