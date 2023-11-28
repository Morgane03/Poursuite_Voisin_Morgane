using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    private readonly Transform[] waypoints;
    private int currentWaypointIndex;
    private NavMeshAgent navMeshAgent;

    public PatrolState(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }

    public void OnEnter(StateController controller)
    {
        // Initialisation du NavMeshAgent et du premier waypoint
        navMeshAgent = controller.GetComponent<NavMeshAgent>();
        currentWaypointIndex = 0;
        SetDestination();
    }

    public void UpdateState(StateController controller)
    {
        // Si le NavMeshAgent a atteint son waypoint actuel, on passe au waypoint suivant
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            SetDestination();
        }
        
        // Vérification si le joueur est à distance faible pour passer en mode Poursuite
        if (MathHelper.VectorDistance(controller.transform.position, controller.player.transform.position) < controller.pursueDistance)
        {
            controller.ChangeState(controller.chaseState);
        }
    }
    public void OnHurt(StateController controller)
    {
        // Pas d'action spécifique lorsqu'on est blessé en mode Patrouille car pas possible
    }
    public void OnExit(StateController controller)
    {
        // Pas d'action spécifique lorsqu'on quitte le mode Patrouille
    }

    public void SetDestination()
    {
        // Définition de la destination du NavMeshAgent vers le waypoint actuel
        navMeshAgent.destination = waypoints[currentWaypointIndex].position;
    }
}
