using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int throwCount;
    [SerializeField] private float throwCooldown;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwUpwardForce = 0.5f;

    [Header("States")]
    public bool aiming;
    public Vector3 aimedTowards;

    [Header("References")]
    [SerializeField] private Transform projectilePrefab;
    
    // reference to the movement functionality
    private PlayerDetails _playerDetails;
    private IsoMovement _movementHandler;
    // private states
    private bool _canThrow = true;

    private void Start()
    {
        _playerDetails = GetComponent<PlayerDetails>();
        _movementHandler = GetComponent<IsoMovement>();
    }

    private void Update()
    {
        // if holding right click and not aiming, set state to aiming
        if (Input.GetMouseButtonDown(1) && !aiming)
            SetAiming(true);

        else if (Input.GetMouseButtonUp(1) && aiming)
        {
            FireProjectile();
            SetAiming(false);
        }
    }

    private void SetAiming(bool _aiming)
    {
        // set state to aiming
        aiming = _aiming;
        _movementHandler.aimInfluenced = _aiming;
    }
    
    private void FireProjectile()
    {
        if (!_canThrow) return;
        
        // create thrown object
        var newProjectile = Instantiate(projectilePrefab, null, true);
        newProjectile.position = _playerDetails.projectileThrowPoint.position;
        
        // get the rigidbody and calculate its force
        Rigidbody newProjectileRb = newProjectile.GetComponent<Rigidbody>();
        Vector3 newThrowForce = (_movementHandler.playerModel.position + transform.forward) * 
            throwForce;
        newThrowForce.y = 0;
        
        // apply force
        newProjectileRb.AddForce(newThrowForce, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        _canThrow = true;
    }
}
