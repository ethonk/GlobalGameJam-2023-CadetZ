using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        
        GameManager.Instance.ChangeGameState(GameManager.GameState.GameOver);
    }
}
