using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelRespawn : MonoBehaviour
{
    public GameObject barrelPrefab; // Prefab do barril explosivo
    public float respawnTime = 5.0f; // Tempo para o respawn do barril após a explosão

    [System.Serializable]
    public class BarrelGroup
    {
        public string groupName;
        public List<Transform> respawnPositions;
        public Collider2D activatorCollider;
    }

    public List<BarrelGroup> barrelGroups; // Lista de grupos de barris

    private Dictionary<int, GameObject> activeBarrels = new Dictionary<int, GameObject>(); // Dicionário de barris ativos

    void Start()
    {
        foreach (var group in barrelGroups)
        {
            if (group.respawnPositions.Count > 0)
            {
                for (int i = 0; i < group.respawnPositions.Count; i++)
                {
                    SpawnBarrelAtPosition(barrelGroups.IndexOf(group), i);
                }
            }
        }
    }

    public void ToDestroyBarrel(GameObject barrel, int groupIndex, int spawnIndex)
    {
        if (barrel != null)
        {
            int key = GetBarrelKey(groupIndex, spawnIndex);
            if (activeBarrels.ContainsKey(key))
            {
                Destroy(barrel);
                activeBarrels.Remove(key);
            }
        }

        StartCoroutine(RespawnBarrel(groupIndex, spawnIndex));
    }

    private void SpawnBarrelAtPosition(int groupIndex, int index)
    {
        BarrelGroup group = barrelGroups[groupIndex];

        if (index >= 0 && index < group.respawnPositions.Count)
        {
            Transform spawnPosition = group.respawnPositions[index];
            GameObject barrel = Instantiate(barrelPrefab, spawnPosition.position, spawnPosition.rotation, transform);

            ExplosionTrap explosiveBarrel = barrel.GetComponent<ExplosionTrap>();
            if (explosiveBarrel != null)
            {
                explosiveBarrel.SetGroupIndex(barrelGroups.IndexOf(group));
                explosiveBarrel.SetSpawnIndex(index);
            }

            int key = GetBarrelKey(barrelGroups.IndexOf(group), index);
            activeBarrels[key] = barrel;
        }
    }

    private IEnumerator RespawnBarrel(int groupIndex, int spawnIndex)
    {
        yield return new WaitForSeconds(respawnTime);

        if (groupIndex >= 0 && groupIndex < barrelGroups.Count)
        {
            BarrelGroup group = barrelGroups[groupIndex];
            if (spawnIndex >= 0 && spawnIndex < group.respawnPositions.Count)
            {
                SpawnBarrelAtPosition(groupIndex, spawnIndex);
            }
        }
    }

    private int GetBarrelKey(int groupIndex, int spawnIndex)
    {
        return groupIndex * 1000 + spawnIndex; // Assume que cada grupo tem menos de 1000 barris
    }
    
    public void ActivateBarrelGroup(Collider2D activatorCollider)
    {
        foreach (var group in barrelGroups)
        {
            if (group.activatorCollider == activatorCollider)
            {
                foreach (var barrel in activeBarrels.Values)
                {
                    ExplosionTrap explosiveBarrel = barrel.GetComponent<ExplosionTrap>();
                    if (explosiveBarrel != null && explosiveBarrel.GetGroupIndex() == barrelGroups.IndexOf(group))
                    {
                        explosiveBarrel.ActivateTrap();
                    }
                }
            }
        }
    }
}