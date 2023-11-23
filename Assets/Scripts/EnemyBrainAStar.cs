using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBrainAStar : MonoBehaviour
{
    public List<GameObject> waypoints;
    public List<GameObject> path;
    public GameObject finalTarget;
    public GameObject targetClosestWaypoint;
    public GameObject currentTarget;
    public int moveSpeed;


    private void Awake()
    {
        targetClosestWaypoint = FindClosestWaypoint(finalTarget);
        DefineWaypointsH();
        currentTarget = FindClosestWaypoint(this.gameObject);
    }
    private void Start()
    {
        ShortestPath(currentTarget);
    }
    void DefineWaypointsH()
    {
        if (waypoints != null)
        {
            foreach (GameObject waypoint in waypoints)
            {
                Waypoint waypointSpt = waypoint.GetComponent<Waypoint>();
                waypointSpt.heurstic = MathHelper.VectorDistance(waypoint.transform.position, finalTarget.transform.position);
            }

        }
    }


    private void FixedUpdate()
    {
        MoveTarget();
        CheckIfTargetMoved();
    }

    void CheckIfTargetMoved()
    {
        GameObject actualPlayerWaypoint = FindClosestWaypoint(finalTarget);
        if (FindClosestWaypoint(finalTarget) != targetClosestWaypoint)
        {
            targetClosestWaypoint = actualPlayerWaypoint;
            path.Clear();
            ShortestPath(currentTarget);
        }
    }

    void MoveTarget()
    {
        if (currentTarget != null)
        {
            Vector3 distance = currentTarget.transform.position - transform.position;
            if (MathHelper.VectorDistance(currentTarget.transform.position, transform.position) <= 0.3f)
            {
                path.Remove(currentTarget);
                if (path.Count() > 0)
                {
                    currentTarget = path[0];
                    return;
                }
                else
                {
                    return;
                }
            }
            transform.LookAt(currentTarget.transform.position);
            Vector3 velocite = moveSpeed * Time.deltaTime * distance.normalized;
            transform.Translate(velocite, Space.World);
        }
    }

    GameObject FindClosestWaypoint(GameObject target)
    {
        GameObject closestWaypoint = null;
        float distanceToClosestWaypoint = 0;
        foreach (GameObject waypoint in waypoints)
        {
            if (closestWaypoint == null)
            {
                closestWaypoint = waypoint;
                distanceToClosestWaypoint = MathHelper.VectorDistance(target.transform.position, waypoint.transform.position);
            }
            else
            {
                float distanceToWaypoint = MathHelper.VectorDistance(target.transform.position, waypoint.transform.position);
                if (distanceToWaypoint < distanceToClosestWaypoint)
                {
                    closestWaypoint = waypoint;
                    distanceToClosestWaypoint = distanceToWaypoint;
                }
            }
        }
        return closestWaypoint;
    }

    void ShortestPath(GameObject start)
    {
        Stack<GameObject> OpenWaypoints = new Stack<GameObject>();
        List<GameObject> ClosedWaypoints = new List<GameObject>();
        OpenWaypoints.Push(start);
        int count = 0;
        while (OpenWaypoints.Count > 0 && count < 1000)
        {
            GameObject currentWP = OpenWaypoints.Pop();
            Waypoint currentWpScript = currentWP.GetComponent<Waypoint>();

            //Debug.Log($"Waypoint Actuel :" + currentWP.name);

            if (currentWP == targetClosestWaypoint)
            {
                //reconstituer le chemin
                Stack<GameObject> totalPath = ReBuildPath(currentWP);
                while (totalPath.Count > 0)
                {
                    path.Add(totalPath.Pop());
                }
                path.Add(finalTarget);
                return;
            }

            foreach (GameObject waypoint in currentWpScript.closeWaypont)
            {
                Waypoint wpScript = waypoint.GetComponent<Waypoint>();
                if (!ClosedWaypoints.Contains(waypoint) && (wpScript.GetComponent<Waypoint>().fNumber <= currentWpScript.fNumber))
                {
                    wpScript.cost += 1;
                    wpScript.fNumber = wpScript.heurstic + wpScript.cost;
                    OpenWaypoints.Push(waypoint);
                    wpScript.opener = currentWP;
                }
            }
            ClosedWaypoints.Add(currentWP);
            count++;
        }
        return;
    }

    Stack<GameObject> ReBuildPath(GameObject FinalWaypoint)
    {
        Stack<GameObject> Pathing = new Stack<GameObject>();
        GameObject ActualWaypoint = FinalWaypoint;
        Pathing.Push(ActualWaypoint);
        int count = 0;
        while (ActualWaypoint.GetComponent<Waypoint>().opener != null && count < 1000)
        {
            ActualWaypoint = ActualWaypoint.GetComponent<Waypoint>().opener;
            Pathing.Push(ActualWaypoint);
            count++;
        }
        return Pathing;
    }
}
