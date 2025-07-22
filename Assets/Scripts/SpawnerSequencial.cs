using System.Collections;
using UnityEngine;

public class SpawnerSequencial : MonoBehaviour
{
    [Header("Objeto a ser instanciado")]
    public GameObject objetoPrefab;

    [Header("Pontos de spawn (ordem importa)")]
    public Transform[] pontos;

    [Header("Controle de tempo")]
    public float tempoPermanencia = 3f;
    public float tempoEntreSpawns = 4f;

    private int indexAtual = 0;

    void Start()
    {
        StartCoroutine(ControlarInstancias());
    }

    IEnumerator ControlarInstancias()
    {
        while (true)
        {
            // Instancia o objeto no ponto atual
            Transform pontoAtual = pontos[indexAtual];
            GameObject instancia = Instantiate(objetoPrefab, pontoAtual.position, Quaternion.identity);

            // Espera o tempo de permanência
            yield return new WaitForSeconds(tempoPermanencia);

            // Destroi o objeto
            Destroy(instancia);

            // Espera antes de instanciar o próximo
            yield return new WaitForSeconds(tempoEntreSpawns);

            // Avança para o próximo ponto
            indexAtual = (indexAtual + 1) % pontos.Length;
        }
    }
}
