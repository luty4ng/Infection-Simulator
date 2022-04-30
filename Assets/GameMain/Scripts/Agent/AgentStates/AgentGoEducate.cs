using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;
using Fsm;


public class AgentGoEducate : IState
{
    public AgentAI controller;
    private AgentData agentData;
    private System.Random random = new System.Random(1000);

    public AgentGoEducate(AgentAI agentAI)
    {
        this.controller = agentAI;
        this.agentData = controller.agentData;
    }
    public void Update()
    {

    }
    public void OnEnter()
    {
        if(agentData.identity == AgentIdentity.Citizen)
        {

        }
    }
    public void OnExit()
    {

    }
}

