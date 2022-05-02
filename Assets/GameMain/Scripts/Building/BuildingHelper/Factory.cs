using UnityEngine;

public class Factory : BuildingHelper
{
    public override int Capacity
    {
        get
        {
            return 12;
        }
    }

    public override void OnStart()
    {
        defaultStayTime = 20f;
    }
    public override void OnAgentTick(AgentData agentData)
    {
        // Debug.Log("Enter Factory");
        agentData.hunger -= agentData.hungerDecreaseSpeed * 0.5f * Time.deltaTime;
        agentData.mood -= agentData.hungerDecreaseSpeed * 0.5f * Time.deltaTime;
        agentData.money += 2f * Time.deltaTime;
    }

    public override void OnAgentEnter(AgentData agentData)
    {

    }

    public override void OnAgentExit(AgentData agentData)
    {
        agentData.money = Mathf.Floor(agentData.money);
    }
}