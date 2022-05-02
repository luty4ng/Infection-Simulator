using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;
using Fsm;
using System.Linq;


public class AgentGoBuilding : IState
{
    public AgentAI controller;
    private AgentData agentData;
    private Building currentBuilding;
    private List<Building> buildingList;

    public AgentGoBuilding(AgentAI agentAI)
    {
        this.controller = agentAI;
        this.agentData = controller.agentData;
        this.buildingList = new List<Building>();
    }
    public void Update()
    {
        Building tmpBuilding = GetCurrentBuilding();
        if (currentBuilding != tmpBuilding)
        {
            controller.isRoaming = true;
        }

    }
    public void OnEnter()
    {
        controller.isGoBuilding = false;
        foreach (var buildingType in agentData.targetBuildingType)
        {
            if (GameCenter.current.buildings[buildingType].Count == 0)
            {
                if (buildingType == BuildingHelperType.None)
                    buildingList.Add(null);
                continue;
            }

            foreach (var building in GameCenter.current.buildings[buildingType])
            {
                buildingList.Add(building);
            }
        }


        currentBuilding = GetCurrentBuilding();

        if (currentBuilding == null)
        {
            controller.isRoaming = true;
            return;
        }


        controller.targetTrans = currentBuilding.buildingInput.transform;
        PathFindingManager.current.RequestPath(controller.transform.position, controller.targetTrans.position, controller.OnPathFound);
    }
    public void OnExit()
    {

    }

    private Building GetCurrentBuilding()
    {
        foreach (var building in buildingList)
        {
            if (building == null)
                return null;
            if (building.helper.IsFull)
                continue;
            return building;
        }
        return null;
    }
}

