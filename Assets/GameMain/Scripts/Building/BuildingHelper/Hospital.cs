using UnityEngine;

public class Hospital : BuildingHelper
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
        defaultStayTime = 30f;
    }
    public override void OnAgentTick(AgentData agentData)
    {
        // Debug.Log("Stay Hospital");
        agentData.hunger += 0.5f * Time.deltaTime;
        agentData.mood -= 0.5f * agentData.hungerDecreaseSpeed * Time.deltaTime;
        agentData.virusData.InfectedValue -= agentData.virusData.InfectedValue * 0.1f * Time.deltaTime;
    }

    public override void OnAgentEnter(AgentData agentData)
    {
        agentData.money -= 50f;
    }

    public override void OnAgentExit(AgentData agentData)
    {
        if (agentData.virusData.InfectedValue > 30 && agentData.virusData.InfectedValue <= 60f)
        {
            agentData.infectionType = InfectionType.Recovered;
        }
        else if (agentData.virusData.InfectedValue <= 30f)
        {
            agentData.infectionType = InfectionType.Unidentified;
        }
        agentData.virusData.symptom = Symptom.Mild;
    }
}