using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlerteState : IState
{
    private StateController stateController;
    private static Transform targetPosition;
    private static Vector3 initialTargetPosition;
    private NavMeshAgent navMeshAgent;

    public AlerteState(StateController controller)
    {
        stateController = controller;
    }

    public void OnEnter(StateController controller)
    {
        targetPosition = controller.player.transform;
        initialTargetPosition = targetPosition.position;
        Debug.Log("Le garde est en état d'alerte ! Va vers la position du bruit entendu...");
        navMeshAgent = controller.GetComponent<NavMeshAgent>();
    }

    public void UpdateState(StateController controller)
    {
        // Déplacer le garde vers la position du bruit
        navMeshAgent.destination = initialTargetPosition;

        // Vérifier si le bruit est atteint
        if (MathHelper.VectorDistance(controller.transform.position, initialTargetPosition) < 0.2f)
        {
            if(MathHelper.VectorDistance(controller.transform.position, controller.player.transform.position) < controller.pursueDistance)
            {
                controller.ChangeState(controller.chaseState);
            }
            else
            {
                // Attendre quelques secondes
                controller.StartCoroutine(WaitAndResumePatrol(controller));
            }
        }
    }

    public void OnHurt(StateController controller)
    {
        // Pas d'action spécifique lorsqu'on est blessé en mode Alerte car
    }

    public void OnExit(StateController controller)
    {
        // Réinitialiser la position cible
        targetPosition = null;
        Debug.Log("Le garde a quitté l'état d'alerte !");
    }

    private IEnumerator WaitAndResumePatrol(StateController controller)
    {
        // Attendre quelques secondes
        yield return new WaitForSeconds(3f);

        // Revenir à l'état de patrouille
        controller.ChangeState(controller.patrolState);
    }
}
