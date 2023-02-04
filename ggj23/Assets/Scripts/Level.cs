using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Details")]
    [SerializeField] private int totalSpawnCount;
    [SerializeField] private int spawnRate;
    [SerializeField] private float spawnDelay;
}
