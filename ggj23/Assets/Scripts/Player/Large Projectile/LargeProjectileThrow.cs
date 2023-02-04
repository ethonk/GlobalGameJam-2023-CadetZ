using Unity.VisualScripting;
using UnityEngine;

public class LargeProjectileThrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private LargeProjectileHitbox hitBox;

    [Header("Held Projectile")]
    public Transform heldProjectile;
    [SerializeField] private float pickSpeed = 5f;
    
    public enum ThrowState { Empty, Picking, Picked }
    [SerializeField] private ThrowState throwState = ThrowState.Empty;
    [SerializeField] private float throwSpeed = 3f;

    // references
    private PlayerDetails _playerDetails;
    private IsoMovement _movementHandler;

    private void Start()
    {
        _playerDetails = FindObjectOfType<PlayerDetails>();
        _movementHandler = transform.parent.GetComponent<IsoMovement>();
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
        
        // if there is a rigid body, delete it
        if (heldProjectile.GetComponent<Rigidbody>() != null)
            Destroy(heldProjectile.GetComponent<Rigidbody>());
        
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
        // ROTATE PLAYER TO DIRECTION OF ATTACK
        //
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 dir = new Vector3();
        
        // attempt raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _movementHandler.raycastLayer))
        {
            // adjust the hit point to ignore the y-axis
            Vector3 newHit = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            // using hit point, get direction
            dir = (newHit - transform.position).normalized;
            Quaternion look = Quaternion.LookRotation(dir);

            // snap to rotation
            _movementHandler.playerModel.rotation = look;
        }
        
        //
        // PHYSICS PORTION
        //
        
        // give the rock physics
        var heldRb = heldProjectile.AddComponent<Rigidbody>();
        heldRb.constraints = RigidbodyConstraints.FreezeRotation;
        
        // get direction of player facing
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
