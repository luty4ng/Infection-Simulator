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
        defaultStayTime = 10f;
    }
    public override void OnAgentTick(AgentData agentData)
    {
        agentData.hunger -= agentData.hungerDecreaseSpeed * Time.deltaTime;
    }

    public override void OnAgentEnter(AgentData agentData)
    {
        agentData.money -= 20f;
    }

    public override void OnAgentExit(AgentData agentData)
    {
        if (agentData.virusData.IsInfected)
        {
            agentData.infectionType = InfectionType.Infected;
        }
    }
}