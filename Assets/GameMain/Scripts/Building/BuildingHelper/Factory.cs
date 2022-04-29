using UnityEngine;

public class Factory : BuildingHelper
{
    public override int Capacity
    {
        get
        {
            return 45;
        }
    }
    public override void OnAgentTick(AgentData agentData)
    {
        Debug.Log("Enter Factory");
    }

    public override void OnAgentEnter(AgentData agentData)
    {

    }

    public override void OnAgentExit(AgentData agentData)
    {

    }
}