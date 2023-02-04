using System;
using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private bool aiming;
    [SerializeField] private Vector3 aimedTowards;

    [Header("References")]
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private Transform projectilePrefab;
    
    // reference to the movement functionality
    private IsoMovement _movementHandler;

    private void Start()
    {
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
        
        // set cursor
        if (_aiming) 
            cursorManager.CursorToAim();
        else 
            cursorManager.CursorToNormal();
    }
    
    private void FireProjectile()
    {
        
    }
}
