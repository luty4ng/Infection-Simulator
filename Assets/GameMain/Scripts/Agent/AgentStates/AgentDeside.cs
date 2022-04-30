using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;
using Fsm;


public class AgentDeside : IState
{
    public AgentAI controller;
    private AgentData agentData;
    private System.Random random = new System.Random(1000);

    public AgentDeside(AgentAI agentAI)
    {
        this.controller = agentAI;
        this.agentData = controller.agentData;
    }
    public void Update()
    {

    }
    public void OnEnter()
    {
        agentData.targetBuildingType = BuildingHelperType.None;


        if (agentData.infectionType == InfectionType.Unidentified || agentData.infectionType == InfectionType.Recovered)
        {
            if (agentData.symptom == Symptom.Moderate || agentData.symptom == Symptom.Severe)
            {
                agentData.targetBuildingType = Random.Range(0f, 1f) >= 0.5f ? BuildingHelperType.CheckPoint : BuildingHelperType.Hospital;
                return;
            }

            if (agentData.hunger <= 40)
                agentData.targetBuildingType = BuildingHelperType.Store;
            else if (agentData.mood <= 40)
            {
                float randomNum = Random.Range(0f, 1f);
                if (randomNum < 0.5f)
                    agentData.targetBuildingType = BuildingHelperType.Disneyland;
                else if (randomNum >= 0.5f && randomNum < 0.75f)
                    agentData.targetBuildingType = BuildingHelperType.None;
                else
                    agentData.targetBuildingType = BuildingHelperType.House;
            }
            else if (agentData.money <= 200)
                agentData.targetBuildingType = BuildingHelperType.Factory;
        }
        else if (agentData.infectionType == InfectionType.Infected)
        {
            agentData.targetBuildingType = Random.Range(0f, 1f) >= 0.5f ? BuildingHelperType.House : BuildingHelperType.Hospital;
            if (agentData.hunger <= 20)
                agentData.targetBuildingType = BuildingHelperType.Store;
            else if (agentData.mood <= 20)
                agentData.targetBuildingType = Random.Range(0f, 1f) >= 0.5f ? BuildingHelperType.House : BuildingHelperType.None;
            else if (agentData.money <= 100)
                agentData.targetBuildingType = BuildingHelperType.Factory;
        }
        controller.isDeside = true;
    }
    public void OnExit()
    {

    }
}

