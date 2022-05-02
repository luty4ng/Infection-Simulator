using UnityEngine;
using System.Collections.Generic;
using GameKit;
public class GameCenter : MonoSingletonBase<GameCenter>
{
    public static bool TIME_ACTIVE = true;
    public static int TIME_MULTIPILER = 1;
    [Range(30, 60)] public float timePerDay = 60;
    public int currentDay = 1;
    public float currentTime = 0;
    public Transform AgentsParent;
    public Transform BuildingParent;
    public Building housePrototype;
    public AgentAI agentPrototype;
    public List<Vector2Int> unwalkblePos = new List<Vector2Int>();
    public Dictionary<BuildingHelperType, List<Building>> buildings = new Dictionary<BuildingHelperType, List<Building>>();
    public bool IsMorning
    {
        get { return currentTime < 20f; }
    }
    public bool IsNoon
    {
        get { return currentTime >= 20f && currentTime < 40; }
    }

    public bool IsNight
    {
        get { return currentTime >= 40f && currentTime < 60f; }
    }

    public Vector2 mouseClickPos
    {
        get
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(worldPos.x, worldPos.y);
        }
    }
    private void Start()
    {
        buildings.Add(BuildingHelperType.None, new List<Building>());
    }
    public void RegisterUnwalkable(Vector2Int pos)
    {
        if (!unwalkblePos.Contains(pos))
        {
            unwalkblePos.Add(pos);
        }
    }
    public void RemoveUnwalkable(Vector2Int pos)
    {
        if (unwalkblePos.Contains(pos))
        {
            unwalkblePos.Remove(pos);
        }
    }

    public void RegisterBuilding(BuildingHelperType type, Building buiding)
    {
        if (!buildings.ContainsKey(type))
            buildings.Add(type, new List<Building>());
        buildings[type].Add(buiding);
    }

    public bool CheckWalkable(Vector2Int pos)
    {
        return !unwalkblePos.Contains(pos);
    }
    public bool CheckWalkable(int x, int y)
    {
        return CheckWalkable(new Vector2Int(x, y));
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timePerDay)
        {
            currentTime = 0;
            currentDay++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateInfectedAgent();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateHealthyAgent();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateHouse();
        }
    }

    private void CreateHouse()
    {
        Building house = Instantiate(housePrototype, mouseClickPos, Quaternion.identity, BuildingParent);
        house.gameObject.SetActive(true);

        for (int i = 0; i < 9; i++)
        {
            AgentAI agent = Instantiate(agentPrototype, mouseClickPos, Quaternion.identity, AgentsParent);
            agent.gameObject.SetActive(true);
        }
    }

    private void CreateInfectedAgent()
    {
        AgentAI agent = Instantiate(agentPrototype, mouseClickPos, Quaternion.identity, AgentsParent);
        agent.agentData.virusData.InfectedValue = 120;
        agent.isInfected = true;
        agent.gameObject.SetActive(true);
    }

    private void CreateHealthyAgent()
    {
        AgentAI agent = Instantiate(agentPrototype, mouseClickPos, Quaternion.identity, AgentsParent);
        agent.agentData.virusData.InfectedValue = 120;
        agent.isInfected = true;
        agent.gameObject.SetActive(true);
    }
}