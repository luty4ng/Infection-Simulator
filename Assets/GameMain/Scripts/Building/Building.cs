using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public enum BuildingHelperType
{
    None,
    Hospital,
    Factory,
    Store,
    Disneyland,
    CheckPoint,
    House
}

public class Building : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(2, 2);
    public BuildingInput buildingInput;
    public SpriteRenderer spriteRenderer;
    public Transform outputTrans;
    public Transform iconTrans;
    public Canvas canvas;
    public GameObject indicatorPrototype;
    public BuildingHelperType buildingType;
    public BuildingHelper helper;
    public Dictionary<AgentData, BuildingIndicator> agents;
    [SerializeField] private Vector2Int pivot;
    private void Awake()
    {

        canvas = GetComponentInChildren<Canvas>();
        agents = new Dictionary<AgentData, BuildingIndicator>();
        if (buildingType == BuildingHelperType.Hospital)
            helper = gameObject.AddComponent<Hospital>();
        else if (buildingType == BuildingHelperType.Factory)
            helper = gameObject.AddComponent<Factory>();
        else if (buildingType == BuildingHelperType.Store)
            helper = gameObject.AddComponent<Market>();
        else if (buildingType == BuildingHelperType.Disneyland)
            helper = gameObject.AddComponent<Institution>();
        else if (buildingType == BuildingHelperType.CheckPoint)
            helper = gameObject.AddComponent<CheckPoint>();
        else if (buildingType == BuildingHelperType.House)
            helper = gameObject.AddComponent<House>();

        if (helper != null)
        {
            helper.building = this;
        }
    }

    private void Start()
    {
        pivot = new Vector2Int(GetOffest(size.x), GetOffest(size.y));
        PathFindingManager.current.grid.GetXY(iconTrans.position, out int X, out int Y);
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                GameCenter.current.RegisterUnwalkable(new Vector2Int(X + x - pivot.x, Y + y - pivot.y));
            }
        }
        GameCenter.current.RegisterBuilding(buildingType, this);
        this.transform.DOPunchScale(Vector3.one * 1.1f, 0.1f);
    }

    private void OnDestroy()
    {
        PathFindingManager.current.grid.GetXY(iconTrans.position, out int X, out int Y);
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                GameCenter.current.RemoveUnwalkable(new Vector2Int(X + x - pivot.x, Y + y - pivot.y));
            }
        }
    }

    public void RegisterAgent(AgentAI agent)
    {
        agent.agentData.OnEnterBuilding(this);
        BuildingIndicator indicator = GameObject.Instantiate(indicatorPrototype, parent: canvas.transform).GetComponent<BuildingIndicator>();
        indicator.Register(this, agent.agentData);
        indicator.gameObject.SetActive(true);
        agent.ResetState();
        agents.Add(agent.agentData, indicator);
        agent.gameObject.SetActive(false);
    }

    public void RemoveAgent(AgentData agentData)
    {
        if (agents.ContainsKey(agentData))
        {
            agentData.entity.agentData = agentData;
            agentData.entity.transform.position = outputTrans.position;
            agentData.OnExitBuilding();
            // agentData.entity.enabled = false;
            Destroy(agents[agentData].gameObject);
            agents.Remove(agentData);
            agentData.entity.gameObject.SetActive(true);
        }
    }
    private int GetOffest(int value)
    {
        if (value == 2)
            return 0;
        return (int)((float)value / 2f);
    }

# if UNITY_EDITOR
    private void OnValidate()
    {
        if (buildingInput != null)
            buildingInput.building = this;
    }
# endif
}