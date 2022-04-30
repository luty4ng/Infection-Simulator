using UnityEngine;

public class CheckPoint : BuildingHelper
{
    public override int Capacity
    {
        get
        {
            return 4;
        }
    }

    public override void OnStart()
    {
        defaultStayTime = 20f;
    }
    public override void OnAgentTick(AgentData agentData)
    {
        Debug.Log("Enter CheckPoint");
    }

    public override void OnAgentEnter(AgentData agentData)
    {

    }

    public override void OnAgentExit(AgentData agentData)
    {

    }
}