using Managers;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // destroy the enemy
        if (!other.CompareTag("Enemy")) return;
        Destroy(other.gameObject);
        
        // run award kill
        GameManager.Instance.AwardKill();
    }
}
