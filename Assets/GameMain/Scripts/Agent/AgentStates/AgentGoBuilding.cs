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
    private List<Building> targetBuildings;
    private Building currentBuilding;

    public AgentGoBuilding(AgentAI agentAI)
    {
        this.controller = agentAI;
        this.agentData = controller.agentData;
        this.targetBuildings = new List<Building>();
    }
    public void Update()
    {
        if (targetBuildings.Count == 0)
        {
            controller.agentData.targetBuildingType = BuildingHelperType.None;
            return;
        }
        float distance = (controller.transform.position - controller.targetTrans.position).magnitude;
        if (distance <= 0.01f)
        {
            Debug.Log("Guess it's full.");
            targetBuildings.Remove(currentBuilding);
            if (targetBuildings.Count == 0)
            {
                controller.agentData.targetBuildingType = BuildingHelperType.None;
                return;
            }
            currentBuilding = targetBuildings.FirstOrDefault();
            controller.targetTrans = currentBuilding.buildingInput.transform;
            PathFindingManager.current.RequestPath(controller.transform.position, controller.targetTrans.position, controller.OnPathFound);
        }
    }
    public void OnEnter()
    {
        targetBuildings = new List<Building>(GameCenter.current.buildings[agentData.targetBuildingType]);
        if (targetBuildings.Count == 0)
        {
            Debug.Log(targetBuildings.Count);
            controller.agentData.targetBuildingType = BuildingHelperType.None;
            return;
        }
        currentBuilding = targetBuildings.FirstOrDefault();
        controller.targetTrans = currentBuilding.buildingInput.transform;
        PathFindingManager.current.RequestPath(controller.transform.position, controller.targetTrans.position, controller.OnPathFound);
    }
    public void OnExit()
    {

    }
}

