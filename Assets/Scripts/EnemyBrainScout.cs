using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBrainScout : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed;
    public float speedRotation;
    private int currentWaypoint = 0;
    private Transform newCurrentWaypoint;

    public void Update()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 waypointsPosition = waypoints[currentWaypoint].position;

        float distance = MathHelper.VectorDistance(enemyPosition, waypointsPosition);

        transform.position = Vector3.MoveTowards(enemyPosition, waypointsPosition, moveSpeed * Time.deltaTime);

        // rotation
        Quaternion targetRotation = Quaternion.LookRotation(waypointsPosition - enemyPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);

        // Passage au waypoint suivant en boucle
        if (distance < 0.2f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
            newCurrentWaypoint = waypoints[currentWaypoint];
        }
    }
}
