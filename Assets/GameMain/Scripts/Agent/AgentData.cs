using UnityEngine;

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
    public float money;
    [Range(0, 100)] public float hunger;
    [Range(0, 100)] public float mood;
    [Range(0.5f, 1.5f)] public float speedFactor;
    [Range(0.5f, 4f)] public float hungerDecreaseSpeed;
    [Range(0.2f, 2f)] public float moodDecreaseSpeed;
    public InfectionType infectionType;
    public Symptom symptom;
    public BuildingHelperType targetBuildingType;
    public VirusData virusData;
    public float countTime = 0f;
    private float checkCrazyTime = 0f;
    Building currentBuilding;


    public AgentData(AgentAI entity, float money = 400, float hunger = 70, float mood = 50, float speedFactor = 1f, float hungerDecreaseSpeed = 1f, float moodDecreaseSpeed = 0.5f)
    {
        this.agentId = Utilities.GetRandomID();
        this.entity = entity;
        this.money = money;
        this.hunger = hunger;
        this.mood = mood;
        this.speedFactor = speedFactor;
        this.hungerDecreaseSpeed = hungerDecreaseSpeed;
        this.moodDecreaseSpeed = moodDecreaseSpeed;
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

    public void OnStayScene()
    {
        hunger -= hungerDecreaseSpeed * Time.deltaTime;
        mood -= moodDecreaseSpeed * Time.deltaTime;
    }

    public bool CheckCrazy()
    {
        checkCrazyTime += Time.deltaTime;
        if (checkCrazyTime >= 2f)
        {
            checkCrazyTime = 0f;
            if (mood <= 20 || hunger <= 20)
            {
                return Random.Range(0f, 1f) > 0.5f ? true : false;
            }
        }
        return false;
    }
}