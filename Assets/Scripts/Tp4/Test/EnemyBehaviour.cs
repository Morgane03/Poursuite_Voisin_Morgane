using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState
{
    Patrolling,
    Pursuing,
    Talking,
}

public class EnemyBehaviour : MonoBehaviour
{
    public Transform[] patrolWaypoints;
    public float patrolSpeed = 3f;
    public float pursuitSpeed = 6f;
    public float detectionRange = 4f;
    public bool pursuit = false;
    public bool patrol = false;

    private NavMeshAgent agent;
    private EnemyState currentState;
    private Transform playerTransform;
    private int currentPatrolWaypointIndex;

    public float discussionDuration = 2f;
    public bool isDiscussing;
    private Transform guardTransform;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrolling;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        guardTransform = GameObject.FindGameObjectWithTag("OrangeGuard").transform;
        currentPatrolWaypointIndex = 0;
        isDiscussing = false;
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
            case EnemyState.Talking:
                Discuss();
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

        float distanceToGuard = MathHelper.VectorDistance(transform.position, guardTransform.position);
        if (distanceToGuard < detectionRange && !pursuit && !patrol)
        {
            currentState = EnemyState.Talking;
            agent.speed = 0;
        }
    }

    public void Pursue()
    {
        pursuit = true;
        // Code pour poursuivre le joueur
        agent.SetDestination(playerTransform.position);

        // Vérifier si le joueur est trop loin pour repasser en mode "Patrouille"
        float distanceToPlayer = MathHelper.VectorDistance(transform.position, playerTransform.position);
        if (distanceToPlayer > detectionRange)
        {
            pursuit = false;
            currentState = EnemyState.Patrolling;
            agent.speed = patrolSpeed;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (currentState == EnemyState.Pursuing && collision.gameObject.CompareTag("Player")) 
        { currentState = EnemyState.Patrolling; currentPatrolWaypointIndex = 0; } // Le joueur est considéré comme “mort” et le garde reprend alors sa “Patrouille”.
    }

    public void Discuss()
    {
        if (!isDiscussing) {
            patrol = true;
            StartCoroutine(DiscussionTimer());
        }
    }

    IEnumerator DiscussionTimer()
    {
        agent.speed = 0f;
        yield return new WaitForSeconds(discussionDuration);
        currentState = EnemyState.Patrolling; // Revenir à l'état "Patrolling"
        currentPatrolWaypointIndex++; // Passe au prochain point de patrouille
        if (currentPatrolWaypointIndex >= patrolWaypoints.Length) 
        { currentPatrolWaypointIndex = 0; }
        isDiscussing = true;

    }
}
