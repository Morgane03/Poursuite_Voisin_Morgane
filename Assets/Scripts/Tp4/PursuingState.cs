using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursuingState : IState
{

    private NavMeshAgent navMeshAgent;
    private Transform player;

    public PursuingState(NavMeshAgent agent, Transform playerTransform)
    {
        navMeshAgent = agent;
        player = playerTransform;
    }

    public void OnEnter(StateController controller)
    {
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(player.position);
    }
    public void UpdateState(StateController controller)
    {
        // Vérifier si le joueur est encore dans la distance de poursuite
        if (MathHelper.VectorDistance(controller.transform.position, player.position) > controller.pursueDistance)
        {
            controller.ChangeState(controller.patrolState);
        }
    }
    public void OnHurt(StateController controller)
    {
        controller.ChangeState(controller.patrolState);
    }

    
    public void OnExit(StateController controller)
    {
        navMeshAgent.enabled = false;
    }
}
