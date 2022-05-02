using UnityEngine;
using GameKit;

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
    public float money;
    [Range(0, 100)] public float hunger;
    [Range(0, 100)] public float mood;
    [Range(0.5f, 1.5f)] public float speedFactor;
    [Range(0.5f, 4f)] public float hungerDecreaseSpeed;
    [Range(0.2f, 2f)] public float moodDecreaseSpeed;
    public InfectionType infectionType;
    public BuildingHelperType targetBuildingType;
    public VirusData virusData;
    private float buildingCountTime = 0f;
    private float crazyCountTime = 0f;
    private float infectedCountTime = 0f;
    private float deathCountTime = 0f;
    private Building currentBuilding;


    public AgentData(AgentAI entity, float infectedValue = 0, float money = 400, float hunger = 70, float mood = 50, float speedFactor = 1f, float hungerDecreaseSpeed = 1f, float moodDecreaseSpeed = 0.5f)
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
        this.virusData = new VirusData(infectedValue);
    }

    public void OnEnterBuilding(Building building)
    {
        // Debug.Log("Agent Enter Building");
        buildingCountTime = 0f;
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
            virusData.OnVirusUpdateSympton();
            currentBuilding.helper.OnAgentTick(this);
            buildingCountTime += Time.deltaTime;
            float stayTime = currentBuilding.helper.defaultStayTime;
            if (buildingCountTime >= Random.Range(stayTime / 2, stayTime))
            {
                currentBuilding.RemoveAgent(this);
            }
        }
    }

    public void OnStayScene()
    {
        hunger -= hungerDecreaseSpeed * Time.deltaTime;
        mood -= moodDecreaseSpeed * Time.deltaTime;
        virusData.OnVirusUpdateSympton();
        if (CheckDeath())
        {
            EventManager.instance.EventTrigger("Agent Dead");
            entity.DestroySelf();
        }
    }
    public void OnInfected()
    {
        infectedCountTime += Time.deltaTime;
        if (infectedCountTime >= VirusData.SpreadCycle)
        {
            infectedCountTime = 0;
            if (Random.Range(0f, 1f) < VirusData.SpreadRate)
            {
                virusData.InfectedValue += Random.Range(VirusData.SpreadMinIncremental, VirusData.SpreadMaxIncremental);
                virusData.InfectedValue = Mathf.Clamp(virusData.InfectedValue, 0, 200);
            }
        }
    }

    public bool CheckCrazy()
    {
        crazyCountTime += Time.deltaTime;
        if (crazyCountTime >= 2f)
        {
            crazyCountTime = 0f;
            if (mood <= 20 || hunger <= 20)
            {
                return Random.Range(0f, 1f) > 0.5f ? true : false;
            }
        }
        return false;
    }

    public bool CheckDeath()
    {
        deathCountTime += Time.deltaTime;
        if (hunger <= 0)
        {
            if (deathCountTime >= 30f)
            {
                deathCountTime = 0f;
                return true;
            }
        }
        return false;
    }
}