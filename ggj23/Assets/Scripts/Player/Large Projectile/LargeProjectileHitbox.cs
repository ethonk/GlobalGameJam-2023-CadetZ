using System.Collections.Generic;
using UnityEngine;

public class LargeProjectileHitbox : MonoBehaviour
{
    public List<Transform> largeProjectilesInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Throwable")) return;
        if (InList(other.transform.parent)) return;
        
        // if pass, add to list
        largeProjectilesInRange.Add(other.transform.parent);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.CompareTag("Throwable")) return;
        if (!InList(other.transform.parent)) return;
        
        // if pass, remove from list
        largeProjectilesInRange.Remove(other.transform.parent);
    }

    private bool InList(Transform obj)
    {
        foreach (Transform p in largeProjectilesInRange)
        {
            if (p == obj) return true;
        }

        return false;
    }

    public Transform GetClosestTransform()
    {
        foreach (Transform projectile in largeProjectilesInRange)
        {
            if (projectile != null) return projectile;
        }

        return null;
    }
    
    public void TakeFromList(Transform projectile)
    {
        foreach (Transform p in largeProjectilesInRange)
        {
            if (p == projectile)
            {
                largeProjectilesInRange.Remove(p);
                return;
            }
        }
    }
}
