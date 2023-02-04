using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float dashStaggerTime = 0.3f;

    [Header("References")]
    [SerializeField] private Transform hitBox;

    [Header("States")]
    public bool killedWithAttack;
    public bool canAttack = true;
    
    // 
    private IsoMovement _movementHandler;
    //
    private float _attackCooldown;
    private float _hitboxUptime;
    
    // timers / states
    private bool _hitboxActive;

    private float _currAttackTimer;
    private float _currHitboxUptime;

    private void Start()
    {
        var plrDetails = GetComponent<PlayerDetails>();

        _movementHandler = GetComponent<IsoMovement>();
        
        _attackCooldown = plrDetails.attackDelay;
        _hitboxUptime = plrDetails.meleeHitboxUptime;
        
        hitBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        // attack key
        if (Input.GetMouseButtonDown(0) && !_movementHandler.aimInfluenced) Attack();

        // managing the cooldown of the hit-box
        if (_hitboxActive)
        {
            _currHitboxUptime += Time.deltaTime;

            if (_currHitboxUptime > _hitboxUptime)
            {
                //
                // DISABLE THE HIT-BOX
                //
                
                _hitboxActive = false;
                hitBox.gameObject.SetActive(false);
                _currHitboxUptime = 0f;
                
                //
                // HANDLING DASH-KILL RESETS
                //
                
                // if the player killed in this uptime, un-encumber and allow attack
                if (!killedWithAttack) return;
                
                // enable can attack
                canAttack = true;
                _currAttackTimer = 0f;
                
                // disable encumber
                _movementHandler.UnEncumber();
                
                // reset killed with attack
                killedWithAttack = false;
            }
        }
        
        // managing cooldown of the attack key
        if (!canAttack)
        {
            _currAttackTimer += Time.deltaTime;

            if (_currAttackTimer > _attackCooldown)
            {
                canAttack = true;
                
                _currAttackTimer = 0f;
            }
        }
    }

    private void Attack()
    {
        // ensure validity of the attack call
        if (!canAttack || _hitboxActive) return;
        canAttack = false;
        
        //
        // ROTATE PLAYER TO DIRECTION OF ATTACK
        //
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // attempt raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _movementHandler.raycastLayer))
        {
            // adjust the hit point to ignore the y-axis
            Vector3 newHit = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            // using hit point, get direction
            Vector3 dir = (newHit - transform.position).normalized;
            Quaternion look = Quaternion.LookRotation(dir);

            // snap to rotation
            transform.rotation = look;
        }
        
        //
        // RUN ATTACK
        //

        // dash player
        _movementHandler.Dash();
        
        // encumber player
        _movementHandler.Encumber(dashStaggerTime);
        
        // activate hit box
        hitBox.gameObject.SetActive(true);
        _hitboxActive = true;
        }
}
