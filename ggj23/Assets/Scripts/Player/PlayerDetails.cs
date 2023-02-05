using System;
using Managers;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    [Header("Player Settings")]
    public float attackDelay = 1.0f;
    public float meleeHitboxUptime = 0.2f;

    [Header("Animator")]
    public Animator playerAnimator;

    [Header("References")]
    public bool isChimpanzee;
    public LargeProjectileThrow heavyThrow;
    public Transform dummy; // corpse

    private void Start()
    {
        // transformation sounds
        if (!isChimpanzee)
        {
            SoundManager.Instance.PlaySound("SFX/caveman_transform-1");
            SoundManager.Instance.PlaySound("SFX/caveman_transform-2");
            SoundManager.Instance.PlaySound("SFX/caveman_transform-3");
        }
        else 
            SoundManager.Instance.PlaySound("SFX/orangu_transform");
    }
}
