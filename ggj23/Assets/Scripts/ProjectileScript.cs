using Managers;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Header("States")]
    public bool airborne;
    
    private void OnTriggerEnter(Collider other)
    {
        DetectFloor(other);
        DetectEnemy(other);
    }

    private void DetectFloor(Collider other)
    {
        if (!other.CompareTag("AimCollision") || !airborne) return;

        // set to not airborne
        airborne = false;
        
        // destroy rigid body
        Destroy(transform.GetComponent<Rigidbody>());
        
        // add screen shake
        EffectsManager.Instance.Shake();
        
        // play sound
        SoundManager.Instance.PlaySound("SFX/caveman_transform-2");
    }
    
    private void DetectEnemy(Collider other)
    {
        // when hitting an enemy and ONLY when airborne, kill them
        if (!other.CompareTag("Enemy") || !airborne) return;
        Destroy(other.gameObject);
        
        // call game manager to award player with kill
        GameManager.Instance.AwardKill(); ;
    }
}
