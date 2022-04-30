using UnityEngine;

public enum AgentIdentity
{
    Citizen,
    Doctor,
    Officer,
    Worker
}
public enum InfectionType
{
    Unidentified,
    Infected,
    Recovered
}

public enum Symptom
{
    None,
    Mild,
    Moderate,
    Severe
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
    [Range(0.5f, 4f)] public float hungerDecreaseSpeed;
    [Range(0.2f, 2f)] public float moodIncreaseSpeed;
    public AgentIdentity identity;
    public InfectionType infectionType;
    public Symptom symptom;
    public BuildingHelperType targetBuildingType;
    public bool isTargetAgent = false;
    public VirusData virusData;
    public float stayTime = 3f;
    public float countTime = 0f;
    Building currentBuilding;


    public AgentData(AgentAI entity, float health = 70, float hunger = 70, float mood = 50, float speedFactor = 1f, float hungerDecreaseSpeed = 1f)
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
        this.symptom = Symptom.None;
        this.virusData = new VirusData();
    }

    public void OnEnterBuilding(Building building)
    {
        // Debug.Log("Agent Enter Building");
        countTime = 0f;
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
            countTime += Time.deltaTime;
            if (countTime >= currentBuilding.helper.defaultStayTime)
            {
                currentBuilding.RemoveAgent(this);
            }
        }
    }

    public void OnUpdate()
    {
        hunger -= hungerDecreaseSpeed * Time.deltaTime;
        mood += moodIncreaseSpeed * Time.deltaTime;
    }

    public bool CheckCrazy()
    {
        return false;
    }
}