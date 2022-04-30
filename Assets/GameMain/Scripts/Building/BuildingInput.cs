using UnityEngine;

public class BuildingInput : MonoBehaviour
{
    public Building building;
    public bool IsFull
    {
        get
        {
            if (building == null)
                return false;
            return building.helper.IsFull;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
        {
            AgentAI agentAI = other.gameObject.GetComponent<AgentAI>();
            if (agentAI.targetTrans == building.buildingInput.transform)
            {
                if(!building.helper.IsFull)
                    building.RegisterAgent(agentAI);
            }
        }
    }




}