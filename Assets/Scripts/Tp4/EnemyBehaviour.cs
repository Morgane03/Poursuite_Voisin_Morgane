using UnityEngine;
using UnityEngine.AI;


public enum EnemyState
{
    Patrolling,
    Pursuing,
}

public class EnemyBehaviour : MonoBehaviour
{
    public Transform[] patrolWaypoints;
    public float patrolSpeed = 3f;
    public float pursuitSpeed = 6f;
    public float detectionRange = 4f;

    private NavMeshAgent agent;
    private EnemyState currentState;
    private Transform playerTransform;
    private int currentPatrolWaypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrolling;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentPatrolWaypointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Pursuing:
                Pursue();
                break;
        }
    }

    public void Patrol()
    {
        agent.speed = patrolSpeed;
        if (patrolWaypoints.Length == 0) { return; }

        agent.SetDestination(patrolWaypoints[currentPatrolWaypointIndex].position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // formule venant d'un ancien tp
            currentPatrolWaypointIndex = (currentPatrolWaypointIndex + 1) % patrolWaypoints.Length;
        }

        float distanceToPlayer = MathHelper.VectorDistance(transform.position, playerTransform.position);
        if (distanceToPlayer < detectionRange)
        {
            currentState = EnemyState.Pursuing;
            agent.speed = pursuitSpeed;
        }
    }

    public void Pursue()
    {
        // Code pour poursuivre le joueur
        agent.SetDestination(playerTransform.position);

        // Vérifier si le joueur est trop loin pour repasser en mode "Patrouille"
        float distanceToPlayer = MathHelper.VectorDistance(transform.position, playerTransform.position);
        if (distanceToPlayer > detectionRange)
        {
            currentState = EnemyState.Patrolling;
            agent.speed = patrolSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == EnemyState.Pursuing && collision.gameObject.CompareTag("Player"))
        {
            // Le joueur est considéré comme “mort” et le garde reprend alors sa “Patrouille”.
            currentState = EnemyState.Patrolling;
            currentPatrolWaypointIndex = 0;
        }
    }
}
