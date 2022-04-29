using UnityEngine;

public class BuildingInput : MonoBehaviour
{
    public Building building;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Agent"))
        {
            AgentAI agentAI = other.gameObject.GetComponent<AgentAI>();
            if(agentAI.targetTrans == building.buildingInput.transform)
            {
                building.RegisterAgent(agentAI);
            }
        }
    }

    


}