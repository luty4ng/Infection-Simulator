using UnityEngine;

public class Institution : BuildingHelper
{
    public override int Capacity
    {
        get
        {
            return 12;
        }
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