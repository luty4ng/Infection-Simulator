using UnityEngine;

public class House : BuildingHelper
{
    public float defaultStayTime;
    public override int Capacity
    {
        get
        {
            return 9;
        }
    }

    [Range(0f, 1f)] public float hungerDecreaseSpeedMultipier = 0.8f;
    [Range(0f, 20f)] public float moodBuff = 10f;
    [Range(0f, 5f)] public float moodBuffSubstracter = 2f;

    public override void OnAgentTick(AgentData agentData)
    {
        // Debug.Log("Stay House");
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