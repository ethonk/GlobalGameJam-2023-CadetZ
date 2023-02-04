using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform hitBox;

    // 
    private float _attackCooldown;
    private float _hitboxUptime;
    
    // timers / states
    private bool _canAttack = true;
    private bool _hitboxActive;

    private float _currAttackTimer;
    private float _currHitboxUptime;

    private void Start()
    {
        var plrDetails = GetComponent<PlayerDetails>();

        _attackCooldown = plrDetails.attackDelay;
        _hitboxUptime = plrDetails.meleeHitboxUptime;
        
        hitBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        // attack key
        if (Input.GetMouseButtonDown(0)) Attack();

        // managing the cooldown of the hitbox
        if (_hitboxActive)
        {
            _currHitboxUptime += Time.deltaTime;

            if (_currHitboxUptime > _hitboxUptime)
            {
                _hitboxActive = false;
                hitBox.gameObject.SetActive(false);

                _currHitboxUptime = 0f;
            }
        }
        
        // managing cooldown of the attack key
        if (!_canAttack)
        {
            _currAttackTimer += Time.deltaTime;

            if (_currAttackTimer > _attackCooldown)
            {
                _canAttack = true;
                
                _currAttackTimer = 0f;
            }
        }
    }

    private void Attack()
    {
        // ensure validity of the attack call
        if (!_canAttack || _hitboxActive) return;
        _canAttack = false;
        
        // activate hit box
        hitBox.gameObject.SetActive(true);
        _hitboxActive = true;
    }
}
