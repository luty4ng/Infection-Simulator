using UnityEngine;

public enum AgentIdentity
{
    Citizen,
    Doctor,
    Officer,
    Courier
}
public enum InfectionType
{
    Unidentified,
    Infected,
    Recovered
}
[System.Serializable]
public class AgentData
{
    public readonly string agentId;
    public readonly AgentAI entity;
    [Range(0, 100)] public float health;
    [Range(0, 100)] public float hunger;
    [Range(0, 100)] public float mood;
    [Range(0.5f, 1.5f)] public float speedFactor;
    [Range(0.1f, 5)] public float hungerDecreaseSpeed;
    public AgentIdentity identity;
    public InfectionType infectionType;
    public BuildingHelperType targetBuildingType;
    public VirusData virusData;
    public float stayTime = 3f;
    public float textTime = 0f;
    Building currentBuilding;

    public AgentData(AgentAI entity, float health = 70, float hunger = 70, float mood = 70, float speedFactor = 1f, float hungerDecreaseSpeed = 1)
    {
        this.agentId = Utilities.GetRandomID();
        this.health = health;
        this.entity = entity;
        this.hunger = hunger;
        this.mood = mood;
        this.speedFactor = speedFactor;
        this.hungerDecreaseSpeed = hungerDecreaseSpeed;
        this.identity = AgentIdentity.Citizen;
        this.infectionType = InfectionType.Unidentified;
        this.virusData = new VirusData();
    }

    public void OnEnterBuilding(Building building)
    {
        // Debug.Log("Agent Enter Building");
        textTime = 0f;
        currentBuilding = building;
        currentBuilding.helper.OnAgentEnter(this);
    }

    public void OnExitBuilding()
    {
        // Debug.Log("Agent Exit Building");
        currentBuilding.helper.OnAgentExit(this);
        currentBuilding = null;
    }

    public void OnStayBuilding()
    {
        if (currentBuilding != null)
        {
            // Debug.Log("Agent in Building");
            currentBuilding.helper.OnAgentTick(this);
            textTime += Time.deltaTime;
            if (textTime >= stayTime)
            {
                currentBuilding.RemoveAgent(this);
            }
        }
    }

    public void OnBuilding()
    {

    }
}