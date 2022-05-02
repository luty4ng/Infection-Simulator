using UnityEngine;

public class House : BuildingHelper
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

    [Range(0f, 1f)] public float hungerDecreaseSpeedMultipier = 0.8f;
    [Range(0f, 20f)] public float moodBuff = 10f;
    [Range(0f, 5f)] public float moodBuffSubstracter = 2f;

    public override void OnAgentTick(AgentData agentData)
    {
        // Debug.Log("Stay House");
        agentData.hunger -= agentData.hungerDecreaseSpeed * 0.5f * Time.deltaTime;
        agentData.mood += 0.2f * Time.deltaTime;
        agentData.virusData.InfectedValue -= 1f * Time.deltaTime;
    }

    public override void OnAgentEnter(AgentData agentData)
    {
        // Debug.Log("Enter House");
    }

    public override void OnAgentExit(AgentData agentData)
    {
        // Debug.Log("Exit House");
    }

}