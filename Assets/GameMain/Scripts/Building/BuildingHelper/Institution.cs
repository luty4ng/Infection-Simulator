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
        defaultStayTime = 10f;
    }
    public override void OnAgentTick(AgentData agentData)
    {
        // Debug.Log("Stay Disneyland");
        agentData.hunger -= agentData.hungerDecreaseSpeed * 0.8f * Time.deltaTime;
        agentData.mood += 2 * Time.deltaTime;
    }

    public override void OnAgentEnter(AgentData agentData)
    {
        agentData.money -= 50f;
    }

    public override void OnAgentExit(AgentData agentData)
    {

    }
}