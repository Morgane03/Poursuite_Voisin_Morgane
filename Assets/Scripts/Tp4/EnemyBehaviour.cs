using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState
{
    Patrolling,
    Pursuing,
    //Attacking
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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrolling;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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

    }

    public void Pursue()
    {

    }
}
