using UnityEngine;
using UnityEngine.UI;
public class BuildingIndicator : MonoBehaviour
{
    public Image image;
    [SerializeField] private Building building;
    [SerializeField] private AgentData agentData;
    private void Update()
    {
        if (agentData != null)
        {
            agentData.OnStayBuilding();
        }
    }

    public void Register(Building building, AgentData agentData)
    {
        this.building = building;
        this.agentData = agentData;
    }
}