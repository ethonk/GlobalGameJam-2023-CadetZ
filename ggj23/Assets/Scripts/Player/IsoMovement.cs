using System;
using UnityEngine;

public class IsoMovement : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f;
    public float turnSpeed = 360f;

    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashStep = 10f;

    [Header("States")]
    public bool dashing;
    public bool encumbered;             // if true, dont allow to move
    public float encumberedCooldown;    // always changing
    public bool aimInfluenced;

    [Header("References")]
    public Transform playerModel;

    [Header("VFX")]
    [SerializeField] private TrailRenderer dashTrail;
    
    // values / references
    private Vector3 _forward, _right;   // augmented forward and right for iso
    private Camera _mainCam;
    private PlayerDetails _playerDetails;
    public int raycastLayer;

    [SerializeField] private Vector3 dashToPosition;
    
    // cooldowns
    private float _currDashDuration;
    private float _currEncumberedCooldown;
    

    private void Start()
    {
        _mainCam = Camera.main;
        _playerDetails = GetComponent<PlayerDetails>();
        
        raycastLayer = LayerMask.GetMask("AimCollision");
        
        // setup isometric values
        _forward = _mainCam.transform.forward;
        _forward.y = 0;
        _forward = Vector3.Normalize(_forward);
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
        
        // set initial movement values
        turnSpeed = moveSpeed;
    }

    private void Update()
    {
        // process the encumbered cooldown
        ProcessEncumberCooldown();
    }

    private void FixedUpdate()
    {
        // process look
        Look();
        
        // move on input
        if (Input.anyKey) Move();
        else if (_playerDetails.playerAnimator != null)
            _playerDetails.playerAnimator.SetBool("Moving", false);
        
        // process dash
        ProcessDash();
    }


    public void Dash()
    {
        // set to dashing, but ignore if already dashing to avoid exploit
        if (dashing) return;
        
        // set dash position desired to (x) paces 
        dashToPosition = transform.position + (transform.forward * dashStep);

        // set to dashing
        dashing = true;
        
        // enable VFX
        dashTrail.emitting = true;
    }

    private void ProcessDash()
    {
        if (!dashing) return;

        // move towards
        transform.position = Vector3.MoveTowards(transform.position, dashToPosition,
            dashSpeed * Time.deltaTime);
        
        // if position is reached, set to not dashing
        if (Vector3.Distance(transform.position, dashToPosition) <= 0.1f)
        {
            dashing = false;
            
            // disable VFX
            dashTrail.emitting = false;
        }
    }
    
    public void Encumber(float cooldown)
    {
        encumberedCooldown = cooldown;
        encumbered = true;
    }

    public void UnEncumber()
    {
        _currEncumberedCooldown = 0f;
        encumbered = false;
    }

    private void ProcessEncumberCooldown()
    {
        if (!encumbered) return;

        _currEncumberedCooldown += Time.deltaTime;
        if (_currEncumberedCooldown > encumberedCooldown)
        {
            _currEncumberedCooldown = 0;
            encumbered = false;
        }
    }
    
    private void Move()
    {
        // don't move if encumbered!
        if (encumbered) return;
        
        // calculate movement
        Vector3 rightMovement = _right * (speed * Input.GetAxis("Horizontal"));
        Vector3 forwardMovement = _forward * (speed * Input.GetAxis("Vertical"));

        Vector3 heading = Vector3.Normalize(rightMovement + forwardMovement);
        
        // Set new position
        Vector3 newPosition = transform.position;
        newPosition += rightMovement;
        newPosition += forwardMovement;

        // Smoothly move the new position
        transform.forward = heading;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
        
        // Play animation
        if (_playerDetails.playerAnimator != null)
            _playerDetails.playerAnimator.SetBool("Moving", true);
    }

    private void Look()
    {
        if (encumbered) return;
        
        // if influenced by aim, look towards aim location
        if (aimInfluenced)
        {
            RaycastHit hit;
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        
            // attempt raycast
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
            {
                // adjust the hit point to ignore the y-axis
                Vector3 newHit = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                // using hit point, get direction
                Vector3 dir = (newHit - transform.position).normalized;
                Quaternion look = Quaternion.LookRotation(dir);

                // slerp to that rotation (only the model)
                playerModel.rotation = Quaternion.Slerp(playerModel.rotation, look,
                    turnSpeed * Time.deltaTime);
            }
        }

        else
        {
            // slerp to that rotation (only the model)
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, transform.rotation,
                turnSpeed * Time.deltaTime);
        }
    }
}
