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
        float distance = (controller.transform.position - controller.targetTrans.position).magnitude;
        if (distance <= 0.001f)
        {
            Debug.Log("Guess it's full.");
            targetBuildings.Remove(currentBuilding);
            currentBuilding = targetBuildings.FirstOrDefault();
            controller.targetTrans = currentBuilding.buildingInput.transform;
            PathFindingManager.current.RequestPath(controller.transform.position, controller.targetTrans.position, controller.OnPathFound);
        }
    }
    public void OnEnter()
    {
        targetBuildings = GameCenter.current.buildings[agentData.targetBuildingType];
        currentBuilding = targetBuildings.FirstOrDefault();
        
        controller.targetTrans = currentBuilding.buildingInput.transform;
        PathFindingManager.current.RequestPath(controller.transform.position, controller.targetTrans.position, controller.OnPathFound);
        Debug.Log(controller.targetTrans.position);
    }
    public void OnExit()
    {

    }
}

