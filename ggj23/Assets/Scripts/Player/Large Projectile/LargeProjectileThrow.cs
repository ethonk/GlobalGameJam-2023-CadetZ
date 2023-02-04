using System;
using Unity.VisualScripting;
using UnityEngine;

public class LargeProjectileThrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private LargeProjectileHitbox hitBox;

    [Header("Held Projectile")]
    [SerializeField] private Transform heldProjectile;
    [SerializeField] private float pickSpeed = 5f;
    
    public enum ThrowState { Empty, Picking, Picked }
    [SerializeField] private ThrowState throwState = ThrowState.Empty;
    [SerializeField] private float throwSpeed = 3f;

    // references
    private PlayerDetails _playerDetails;

    private void Start()
    {
        _playerDetails = FindObjectOfType<PlayerDetails>();
    }

    private void Update()
    {
        // picking
        if (throwState == ThrowState.Picking) ProcessPick();

        // pick
        if (Input.GetMouseButtonDown(1) && throwState == ThrowState.Empty &&
            hitBox.largeProjectilesInRange.Count > 0)
            Pick(hitBox.TakeFromList());
        
        // throw
        if (Input.GetMouseButtonDown(0) && throwState == ThrowState.Picked)
            Throw();
    }

    public void Pick(Transform obj)
    {
        // check then change state
        if (throwState != ThrowState.Empty) return;
        throwState = ThrowState.Picking;
        
        // set and parent obj
        heldProjectile = obj;
        heldProjectile.SetParent(holdPoint);
        
        // play animation
        _playerDetails.playerAnimator.SetBool("HoldingItem", true);
    }

    private void ProcessPick()
    {
        heldProjectile.position = Vector3.MoveTowards(heldProjectile.position,
            holdPoint.position, pickSpeed * Time.deltaTime);

        if (Vector3.Distance(heldProjectile.position, holdPoint.position) <= 0.1f)
        {
            // set position
            heldProjectile.position = holdPoint.position;
            
            // set new state
            throwState = ThrowState.Picked;
        }
    }
    
    public void Throw()
    {
        // don't throw if we dont have the picked item
        if (throwState != ThrowState.Picked) return;
        
        // un-parent from base
        heldProjectile.SetParent(null);
        
        //
        // PHYSICS PORTION
        //
        
        // give the rock physics
        var heldRb = heldProjectile.AddComponent<Rigidbody>();
        heldRb.constraints = RigidbodyConstraints.FreezeRotation;
        
        // get direction of player facing
        Vector3 dir = transform.position + Vector3.forward;
        heldRb.AddForce(dir * throwSpeed, ForceMode.Impulse);
        
        
        // at the end, set held projectile to null
        heldProjectile = null;
        
        //
        // ANIMATION
        //
        
        // play animation
        _playerDetails.playerAnimator.SetTrigger("Throw");
        _playerDetails.playerAnimator.SetBool("HoldingItem", false);
    }
}
