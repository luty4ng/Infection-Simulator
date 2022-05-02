using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathFind;
using Fsm;

public class AgentAI : MonoBehaviour
{
    [SerializeField] private string currentState;
    [Range(3, 10f)] public float speed = 8f;
    [Range(1f, 5f)] public float detectRadius = 2;
    [Range(1f, 3f)] public float roamingRadius = 2;

    public LayerMask detectLayers;
    public Transform targetTrans;
    public SpriteRenderer spriteRenderer;
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
        stateMachine = new StateMachine();
        agentData = new AgentData(this, isInfected ? 80 : 0);

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
    }

    private void OnEnable()
    {

        if (isStart && agentData != null)
        {
            // PathFindingManager.current.RequestPath(this.transform.position, targetTrans.position, OnPathFound);
            ResetState();
        }
    }

    void Update()
    {
        stateMachine.Update();
        currentState = stateMachine.GetCurrentState().ToString();
        agentData.OnStayScene();
        if (agentData.virusData.IsInfected)
        {
            Collider2D[] coll = Physics2D.OverlapCircleAll(this.transform.position, detectRadius, detectLayers);
            for (int i = 0; i < coll.Length; i++)
            {
                coll[i].gameObject.GetComponent<AgentAI>().agentData.OnInfected();
            }
        }
        spriteRenderer.color = GetUpdatedColor();
    }

    public void ResetState() => stateMachine.SetState(deside);

    public void OnPathFound(List<Vector3> newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            waypoints = newPath;
            waypointIndex = 0;
            if (this.gameObject.activeInHierarchy)
            {
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }

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

    public Color GetUpdatedColor()
    {
        if (agentData.infectionType == InfectionType.Unidentified)
            return new Color(0.21f, 0.85f, 0.55f, 1);
        else if (agentData.infectionType == InfectionType.Infected)
            return new Color(0.98f, 0.36f, 0.4f, 1);
        else if (agentData.infectionType == InfectionType.Recovered)
            return new Color(1f, 0.7f, 0f, 1);
        return new Color(0.21f, 0.85f, 0.55f, 1);
    }

    public void DestroySelf() => Destroy(this.gameObject);

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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, roamingRadius);
    }
}
