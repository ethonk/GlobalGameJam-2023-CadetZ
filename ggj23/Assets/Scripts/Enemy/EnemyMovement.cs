using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    
    private NavMeshAgent agent;
    
    private void Start()
    {
        player = FindObjectOfType<PlayerDetails>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null) return;

        agent.destination = player.position;
    }
}
