using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    [SerializeField] private List<Transform> invalidSpotsColliding;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Throwable") || other.CompareTag("Boundary"))
        {
            AddToList(other.transform);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Throwable") || other.CompareTag("Boundary"))
        {
            RemoveList(other.transform);
        }
    }

    public bool ListPopulated()
    {
        foreach (Transform child in invalidSpotsColliding)
        {
            if (child != null) return true;
        }
        return false;
    }
    
    private void AddToList(Transform obj)
    {
        if (InList(obj)) return;
        
        invalidSpotsColliding.Add(obj);
    }

    private void RemoveList(Transform obj)
    {
        if (!InList(obj)) return;

        invalidSpotsColliding.Remove(obj);
    }

    private bool InList(Transform obj)
    {
        foreach (Transform child in invalidSpotsColliding)
        {
            if (child == obj) return true;
        }

        return false;
    }
}
