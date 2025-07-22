using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    public GameObject barrelPrefab; // Prefab do barril explosivo
    public float respawnTime = 5.0f; // Tempo para o respawn do barril após a explosão
    public List<Transform> respawnPositions; // Lista de posições de respawn

    private Dictionary<int, GameObject> activeBarrels = new Dictionary<int, GameObject>(); // Dicionário de barris ativos

    void Start()
    {
        if (respawnPositions.Count > 0)
        {
            // Para cada posição de respawn, instancie um barril
            for (int i = 0; i < respawnPositions.Count; i++)
            {
                SpawnBarrelAtPosition(i);
            }
        }
        else
        {
            Debug.LogError("Nenhuma posição de respawn definida.");
        }
    }

    public void DestroyBarrel(GameObject barrel, int spawnIndex)
    {
        if (barrel != null)
        {
            Destroy(barrel);
            Debug.Log($"Barrel destroyed at index: {spawnIndex}");
        }

        // Verifica se o índice é válido e se o barril está na lista antes de remover
        if (activeBarrels.ContainsKey(spawnIndex))
        {
            activeBarrels.Remove(spawnIndex);
            Debug.Log($"Barrel removed from active list at index: {spawnIndex}");
        }
        else
        {
            Debug.LogWarning($"Attempted to remove barrel from active list at invalid index: {spawnIndex}");
        }

        // Inicia uma coroutine para respawnar o barril após um tempo
        StartCoroutine(RespawnBarrel(spawnIndex));
    }

    private void SpawnBarrelAtPosition(int index)
    {
        if (index >= 0 && index < respawnPositions.Count)
        {
            // Pega a posição de respawn correspondente ao índice fornecido
            Transform spawnPosition = respawnPositions[index];
            Debug.Log($"Respawnando barril na posição {index}: {spawnPosition.position}");

            // Instancia um novo barril na posição de respawn
            GameObject barrel = Instantiate(barrelPrefab, spawnPosition.position, spawnPosition.rotation, transform);

            // Define o índice de spawn no barril para rastrear sua posição de respawn
            ExplosiveBarrel explosiveBarrel = barrel.GetComponent<ExplosiveBarrel>();
            if (explosiveBarrel != null)
            {
                Debug.Log($"Definindo índice de respawn {index} para o barril");
                explosiveBarrel.SetSpawnIndex(index);
            }

            // Adiciona o barril ao dicionário de barris ativos
            if (activeBarrels.ContainsKey(index))
            {
                Debug.LogError($"Tentativa de adicionar um barril já existente no índice: {index}");
            }
            else
            {
                activeBarrels[index] = barrel;
                Debug.Log($"Barrel added to active list at index: {index}");
            }
        }
        else
        {
            Debug.LogError($"Índice de posição de respawn fora do intervalo: {index}");
        }
    }

    private IEnumerator RespawnBarrel(int spawnIndex)
    {
        // Aguarda o tempo de respawn especificado
        yield return new WaitForSeconds(respawnTime);

        // Verifica se o índice é válido antes de tentar respawnar o barril
        if (spawnIndex >= 0 && spawnIndex < respawnPositions.Count)
        {
            SpawnBarrelAtPosition(spawnIndex);
        }
        else
        {
            Debug.LogError($"Tentativa de respawn em um índice inválido: {spawnIndex}");
        }
    }}
