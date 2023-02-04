using System;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    [Header("Player Settings")]
    public float attackDelay = 1.0f;
    public float meleeHitboxUptime = 0.2f;

    [Header("Animator")]
    public Animator playerAnimator;

    [Header("Holster")]
    public Transform projectileThrowPoint;

    // references
    public bool isChimpanzee;
    private PlayerAttack _meleeAtkHandler;
    private PlayerRangedAttack _rangeAtkHandler;

    private void Start()
    {
        _meleeAtkHandler = GetComponent<PlayerAttack>();
        _rangeAtkHandler = GetComponent<PlayerRangedAttack>();
    }

    private void Update()
    {
        if (isChimpanzee) UpdateCursor();
    }

    void UpdateCursor()
    {
        if (_meleeAtkHandler.canAttack && !_rangeAtkHandler.aiming)
            CursorManager.Instance.CursorToNormal();
        
        else if (!_meleeAtkHandler.canAttack && !_rangeAtkHandler.aiming)
            CursorManager.Instance.CursorToNormalCd();
        
        else if (_rangeAtkHandler.aiming)
            CursorManager.Instance.CursorToAim();
    }
}
