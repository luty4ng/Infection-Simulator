using UnityEngine;

public class Institution : BuildingHelper
{
    public override int Capacity
    {
        get
        {
            return 6;
        }
    }
    public override void OnStart()
    {
        defaultStayTime = 20f;
    }
    public override void OnAgentTick(AgentData agentData)
    {
        Debug.Log("Enter Institution");
    }

    public override void OnAgentEnter(AgentData agentData)
    {

    }

    public override void OnAgentExit(AgentData agentData)
    {

    }
}