using UnityEngine;

public class Hospital : BuildingHelper
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
        Debug.Log("Enter Hospital");
    }

    public override void OnAgentEnter(AgentData agentData)
    {

    }

    public override void OnAgentExit(AgentData agentData)
    {

    }
}