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
    [SerializeField] private string currentState;
    [Range(3, 10f)] public float speed = 8f;
    [Range(1f, 5f)] public float detectRadius = 2;
    [Range(1f, 5f)] public float infectRadius = 2;
    [Range(1f, 3f)] public float roamingRadius = 2;

    public LayerMask detectLayers;
    public Transform targetTrans;
    public bool isInfected = false;
    public bool isDeside = false;
    public AgentData agentData;
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();
    private Transform lastTarget;
    private int waypointIndex;
    private StateMachine stateMachine;
    private bool isStart = false;
    private AgentDeside deside;

    private void Start()
    {
        isStart = true;
        targetTrans = lastTarget = null;
        agentData = new AgentData(this);
        stateMachine = new StateMachine();

        deside = new AgentDeside(this);
        AgentRoaming roaming = new AgentRoaming(this);
        AgentGoBuilding goBuilding = new AgentGoBuilding(this);
        AgentCrazy crazy = new AgentCrazy(this);

        System.Func<bool> GoToBuiding() => () => agentData.targetBuildingType != BuildingHelperType.None;
        System.Func<bool> HasNoTarget() => () => agentData.targetBuildingType == BuildingHelperType.None;
        System.Func<bool> Deside() => () => isDeside;
        System.Func<bool> CheckCrazy() => () => agentData.CheckCrazy();

        stateMachine.AddTransition(deside, goBuilding, GoToBuiding());
        stateMachine.AddTransition(roaming, deside, Deside());
        stateMachine.AddTransition(goBuilding, roaming, HasNoTarget());
        stateMachine.AddTransition(deside, roaming, HasNoTarget());
        stateMachine.AddTransition(crazy, roaming, HasNoTarget());
        stateMachine.AddAnyTransition(crazy, CheckCrazy());
        stateMachine.SetState(roaming);


        // if (targetTrans != null)
        //     PathFindingManager.current.RequestPath(this.transform.position, targetTrans.position, OnPathFound);
    }

    private void OnEnable()
    {
        if (isStart && targetTrans != null && agentData != null)
        {
            PathFindingManager.current.RequestPath(this.transform.position, targetTrans.position, OnPathFound);
        }
    }

    void Update()
    {
        stateMachine.Update();
        currentState = stateMachine.GetCurrentState().ToString();
        agentData.OnStayScene();
        // Collider2D coll = Physics2D.OverlapCircle(this.transform.position, detectRadius, detectLayers);
        // if (coll)
        // {

        // }

    }

    public void ResetState() => stateMachine.SetState(deside);

    public void OnPathFound(List<Vector3> newPath, bool pathSuccessful)
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
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, detectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, infectRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, roamingRadius);
    }
}
