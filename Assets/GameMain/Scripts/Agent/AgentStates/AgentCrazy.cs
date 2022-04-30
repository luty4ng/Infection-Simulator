using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;
using Fsm;


public class AgentCrazy : IState
{
    public AgentAI controller;
    private AgentData agentData;
    private System.Random random = new System.Random(1000);
    private const float stayTime = 30f;
    private float countTime = 0f;
    public AgentCrazy(AgentAI agentAI)
    {
        this.controller = agentAI;
        this.agentData = controller.agentData;
    }
    public void Update()
    {
        countTime += Time.deltaTime;
        if (countTime >= stayTime)
        {
            agentData.targetBuildingType = BuildingHelperType.None;
        }


    }
    public void OnEnter()
    {

    }
    public void OnExit()
    {

    }
}

