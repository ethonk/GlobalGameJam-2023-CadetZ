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

    [Header("References")]
    public bool isChimpanzee;
    public LargeProjectileThrow heavyThrow;
}
