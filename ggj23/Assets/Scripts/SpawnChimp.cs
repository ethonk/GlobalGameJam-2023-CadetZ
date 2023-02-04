using System;
using UnityEngine;

public class SpawnChimp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float descentSpeed = 5f;
    [SerializeField] private float startingY = 15f;
    
    [Header("References")]
    [SerializeField] private Transform chimpPrefab;

    private void Start()
    {
        transform.position += new Vector3(0, startingY, 0);
    }

    private void Update()
    {
        // descend
        transform.position -= new Vector3(0, descentSpeed, 0) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("AimCollision")) return;
        
        // spawn chimp
        var newChimp = Instantiate(chimpPrefab, null);
        newChimp.position = transform.position;

        // destroy self
        Destroy(gameObject);
    }
}
