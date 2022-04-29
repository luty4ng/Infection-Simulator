using UnityEngine;

public abstract class BuildingHelper : MonoBehaviour
{
    public Building building;
    public bool IsFull
    {
        get
        {
            return building.agents.Count >= Capacity;
        }
    }
    public abstract int Capacity { get; }
    public abstract void OnAgentTick(AgentData agentData);
    public abstract void OnAgentEnter(AgentData agentData);
    public abstract void OnAgentExit(AgentData agentData);
}