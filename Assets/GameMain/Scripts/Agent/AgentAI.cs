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
    public AgentData agentData;
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();
    private Transform lastTarget;
    private int waypointIndex;
    private StateMachine stateMachine;
    private bool isStart = false;
    private void Start()
    {
        isStart = true;
        targetTrans = lastTarget = null;
        agentData = new AgentData(this);
        stateMachine = new StateMachine();

        AgentRoaming roaming = new AgentRoaming(this);
        AgentGoHospital toHospital = new AgentGoHospital(this);
        AgentGoCheck toCheckPoint = new AgentGoCheck(this);
        AgentGoWork toWork = new AgentGoWork(this);
        AgentGoEducate toInstitution = new AgentGoEducate(this);
        AgentGoHome toHome = new AgentGoHome(this);
        AgentGoCrazy goCrazy = new AgentGoCrazy(this);
        AgentGoAgent goAgent = new AgentGoAgent(this);

        System.Func<bool> GoToHospital() => () => agentData.targetBuildingType == BuildingHelperType.Hospital;
        System.Func<bool> GoToInstitution() => () => agentData.targetBuildingType == BuildingHelperType.Institution;
        System.Func<bool> GoToHome() => () => agentData.targetBuildingType == BuildingHelperType.House;
        System.Func<bool> GoToCheckPoint() => () => agentData.targetBuildingType == BuildingHelperType.CheckPoint;
        System.Func<bool> GoToFactory() => () => agentData.targetBuildingType == BuildingHelperType.Factory;
        System.Func<bool> GoToAgent() => () => agentData.isTargetAgent;
        System.Func<bool> GoToCrazy() => () => agentData.CheckCrazy();
        System.Func<bool> HasNoTarget() => () => targetTrans == null || (agentData.targetBuildingType == BuildingHelperType.None && !agentData.isTargetAgent);

        stateMachine.AddAnyTransition(toHospital, GoToHospital());
        stateMachine.AddAnyTransition(toCheckPoint, GoToCheckPoint());
        stateMachine.AddAnyTransition(toWork, GoToFactory());
        stateMachine.AddAnyTransition(toInstitution, GoToInstitution());
        stateMachine.AddAnyTransition(toHome, GoToHome());
        stateMachine.AddAnyTransition(goCrazy, GoToCrazy());
        stateMachine.AddAnyTransition(goAgent, GoToAgent());
        stateMachine.AddAnyTransition(roaming, HasNoTarget());
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
        // Collider2D coll = Physics2D.OverlapCircle(this.transform.position, detectRadius, detectLayers);
        // if (coll)
        // {

        // }
        if (agentData.infectionType == InfectionType.Infected)
        {
            agentData.targetBuildingType = Random.Range(0f, 1f) >= 0.5f ? BuildingHelperType.House : BuildingHelperType.Hospital;
        }
        else if (agentData.infectionType == InfectionType.Unidentified)
        {
            agentData.targetBuildingType = Random.Range(0f, 1f) >= 0.5f ? BuildingHelperType.CheckPoint : BuildingHelperType.Hospital;
        }


        if (GameCenter.current.IsMorning)
        {
            if (agentData.identity == AgentIdentity.Citizen)
            {
                agentData.targetBuildingType = Random.Range(0f, 1f) >= 0.5f ? BuildingHelperType.Institution : BuildingHelperType.None;
            }
            else if (agentData.identity == AgentIdentity.Doctor)
            {

            }
            else if (agentData.identity == AgentIdentity.Officer)
            {

            }
            else if (agentData.identity == AgentIdentity.Worker)
            {

            }
        }


    }

    private void ChangeDecision()
    {
        
    }

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
