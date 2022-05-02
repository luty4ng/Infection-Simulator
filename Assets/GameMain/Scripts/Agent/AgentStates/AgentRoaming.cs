using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;
using Fsm;


public class AgentRoaming : IState
{
    public AgentAI controller;
    private AgentData agentData;
    private System.Random random = new System.Random(1000);
    private float pauseTime;
    private float pauseCountTime;
    public AgentRoaming(AgentAI agentAI)
    {
        this.controller = agentAI;
        this.agentData = controller.agentData;
        this.pauseTime = Random.Range(0.5f, 2f);
        this.pauseCountTime = 0;
    }
    public void Update()
    {
        pauseCountTime += Time.deltaTime;
        if (pauseCountTime >= pauseTime)
        {
            pauseCountTime = 0;
            Vector2 targetPos = (Vector2)controller.transform.position + GetCirclePoint(controller.roamingRadius);
            PathFindingManager.current.RequestPath(controller.transform.position, targetPos, controller.OnPathFound);
        }

    }
    public void OnEnter()
    {
        controller.targetTrans = null;
        controller.isRoaming = false;
        agentData.targetBuildingType.Clear();
        if (agentData.infectionType == InfectionType.Unidentified || agentData.infectionType == InfectionType.Recovered)
        {
            if (agentData.virusData.symptom == Symptom.Moderate || agentData.virusData.symptom == Symptom.Severe)
            {
                agentData.targetBuildingType.Add(BuildingHelperType.CheckPoint);
            }

            if (agentData.hunger <= 40)
                agentData.targetBuildingType.Add(BuildingHelperType.Store);
            if (agentData.mood <= 40)
            {
                float randomNum = Random.Range(0f, 1f);
                if (randomNum < 0.5f)
                    agentData.targetBuildingType.Add(BuildingHelperType.Disneyland);
                else if (randomNum >= 0.5f && randomNum < 0.75f)
                    agentData.targetBuildingType.Add(BuildingHelperType.None);
                else
                    agentData.targetBuildingType.Add(BuildingHelperType.House);
            }
            else if (agentData.money <= 200)
                agentData.targetBuildingType.Add(BuildingHelperType.Factory);
        }
        else if (agentData.infectionType == InfectionType.Infected)
        {
            agentData.targetBuildingType.Add(Random.Range(0f, 1f) >= 0.5f ? BuildingHelperType.House : BuildingHelperType.Hospital);
            if (agentData.hunger <= 20)
                agentData.targetBuildingType.Add(BuildingHelperType.Store);
            else if (agentData.mood <= 20)
                agentData.targetBuildingType.Add(Random.Range(0f, 1f) >= 0.5f ? BuildingHelperType.House : BuildingHelperType.None);
            else if (agentData.money <= 100)
                agentData.targetBuildingType.Add(BuildingHelperType.Factory);
        }
        agentData.targetBuildingType.Add(BuildingHelperType.None);
        controller.isGoBuilding = true;
    }
    public void OnExit()
    {

    }

    private Vector2 GetCirclePoint(float m_Radius)
    {
        float radin = (float)GetRandomValue(0, 2 * Mathf.PI);
        float x = m_Radius * Mathf.Cos(radin);
        float y = m_Radius * Mathf.Sin(radin);
        Vector2 endPoint = new Vector2(x, y);
        endPoint = PathFindingManager.current.grid.Clamp(endPoint);
        return endPoint;
    }
    private double GetRandomValue(double min, double max)
    {
        double v = random.NextDouble() * (max - min) + min;
        return v;
    }
}

