using UnityEngine;

public class BuildingIcon : MonoBehaviour
{
    public Building Prototype;
    private BuildingPanel buildingPanel;
    private void Start()
    {
        buildingPanel = GetComponentInParent<BuildingPanel>();
    }
    public void OnClick()
    {
        buildingPanel.CreateBuildingMeta(Prototype);
    }
}