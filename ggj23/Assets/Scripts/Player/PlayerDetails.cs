using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    [Header("Player Settings")]
    public float attackDelay = 1.0f;
    public float meleeHitboxUptime = 0.2f;

    [Header("Holster")]
    public Transform heldProjectile;
}
