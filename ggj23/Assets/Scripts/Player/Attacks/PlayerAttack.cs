using System;
using Managers;
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
    private PlayerDetails _playerDetails;
    private IsoMovement _movementHandler;
    private AttackCheck _attackCheck;
    //
    private float _attackCooldown;
    private float _hitboxUptime;
    
    // timers / states
    private bool _hitboxActive;

    private float _currAttackTimer;
    private float _currHitboxUptime;

    private void Start()
    {
        _playerDetails = GetComponent<PlayerDetails>();

        _movementHandler = GetComponent<IsoMovement>();
        
        _attackCooldown = _playerDetails.attackDelay;
        _hitboxUptime = _playerDetails.meleeHitboxUptime;
        _attackCheck = FindObjectOfType<AttackCheck>();
        
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
        // if a caveman, make sure its not holding an object
        if (!_playerDetails.isChimpanzee && _playerDetails.heavyThrow.heldProjectile != null)
            return;
        
        // ensure that we are attacking from a valid position
        if (_attackCheck.ListPopulated()) return;
        
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
        
        //
        // ANIMATION
        //

        // only if animator
        if (_playerDetails.playerAnimator == null) return;
        
        // roll for a random swing animation
        int rand = UnityEngine.Random.Range(0, 2);
        
        // play animation accordingly
        if (rand == 0) _playerDetails.playerAnimator.SetTrigger("MeleeLeft");
        else _playerDetails.playerAnimator.SetTrigger("MeleeRight");
        
        //
        // SOUND
        //
        
        if (_playerDetails.isChimpanzee) SoundManager.Instance.PlaySound("SFX/orangu_attack");
        else SoundManager.Instance.PlaySound("SFX/caveman_attack");
    }
}
