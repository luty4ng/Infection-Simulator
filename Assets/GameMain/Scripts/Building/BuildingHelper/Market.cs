using UnityEngine;

public class Market : BuildingHelper
{
    public override int Capacity
    {
        get
        {
            return 9;
        }
    }
    public override void OnStart()
    {
        defaultStayTime = 20f;
    }
    public override void OnAgentTick(AgentData agentData)
    {
        Debug.Log("Enter Market");
    }

    public override void OnAgentEnter(AgentData agentData)
    {

    }

    public override void OnAgentExit(AgentData agentData)
    {

    }

}