using Managers;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerAttack meleeAttackHandler;
    
    private void OnTriggerEnter(Collider other)
    {
        // destroy the enemy
        if (!other.CompareTag("Enemy")) return;
        Destroy(other.gameObject);
        
        // run award kill
        GameManager.Instance.AwardKill();
        
        // run reset for melee attacks
        meleeAttackHandler.killedWithAttack = true;
    }
}
