using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawner Details")]
    public int spawnCount = 5;

    [Header("Spawner Settings")]
    [SerializeField] private float spawnDelay = 0.5f;

    [Header("References")] 
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform spawnPositionParent;
    [SerializeField] private List<Transform> spawnPositions;
    
    // private values
    private float currSpawnCooldown;
    
    // values / references
    private Transform _player;

    private void Start()
    {
        // find player
        _player = FindObjectOfType<PlayerDetails>().transform;
        
        // populate spawn positions
        foreach (Transform child in spawnPositionParent)
        {
            spawnPositions.Add(child);
        }
    }

    private void Update()
    {
        if (spawnCount <= 0) return;

        currSpawnCooldown += Time.deltaTime;
        if (currSpawnCooldown >= spawnDelay)
        {
            currSpawnCooldown = 0;
            
            SpawnEnemies(1);
            spawnCount--;
        }
    }

    public void SpawnEnemies(int spawnAmount)
    {
        for (var i = 0; i < spawnAmount; i++)
        {
            // create and set position of the enemy
            var newEnemy = Instantiate(enemyPrefab, transform, true);
            newEnemy.transform.position = ChooseRandomSpawnPosition().position;
        }
    }

    private Transform ChooseRandomSpawnPosition()
    {
        return spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
    }
}
