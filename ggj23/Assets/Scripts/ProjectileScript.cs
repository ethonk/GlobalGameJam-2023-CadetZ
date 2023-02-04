using Managers;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // when hitting enemy, destroy them
        if (!other.CompareTag("Enemy")) return;
        Destroy(other.gameObject);
        
        // call game manager to award player with kill
        GameManager.Instance.AwardKill();
        
        // destroy the projectile
        Destroy(gameObject);
    }
}
