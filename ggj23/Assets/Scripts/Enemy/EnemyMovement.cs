using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    
    private NavMeshAgent agent;
    private Vector3 lastPosition;

    private void Start()
    {
        player = FindObjectOfType<PlayerDetails>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null) player = FindObjectOfType<PlayerDetails>().transform;
        agent.destination = player.position;
    }
}
