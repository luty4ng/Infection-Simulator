using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathFind;
using Fsm;
using Timer;

public class AgentAI : MonoBehaviour
{
    public float speed = 8f;
    public Transform targetTrans;
    public bool isInfected = false;
    public AgentData agentData;
    private List<Vector3> waypoints = new List<Vector3>();
    private int waypointIndex;
    private StateMachine stateMachine;
    private bool isStart = false;
    private void Start()
    {
        isStart = true;
        agentData = new AgentData(this);
        if (targetTrans != null)
            PathFindingManager.current.RequestPath(this.transform.position, targetTrans.position, OnPathFound);
    }

    private void OnEnable()
    {
        if (isStart && targetTrans != null && agentData!=null)
        {
            PathFindingManager.current.RequestPath(this.transform.position, targetTrans.position, OnPathFound);
        }
    }

    void Update()
    {

    }

    private void OnPathFound(List<Vector3> newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            waypoints = newPath;
            waypointIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = waypoints[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                waypointIndex++;
                if (waypointIndex >= waypoints.Count)
                {
                    yield break;
                }
                currentWaypoint = waypoints[waypointIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (waypoints != null)
        {
            for (int i = waypointIndex; i < waypoints.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(waypoints[i], Vector3.one / 2);
                if (i == waypointIndex)
                {
                    Gizmos.DrawLine(transform.position, waypoints[i]);
                }
                else
                {
                    Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
                }
            }

        }
    }
}
