using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingPanel : MonoBehaviour
{
    public BuildingMeta metaBuilding;
    private bool isOpen = true;
    private bool isTweening;
    private void Update()
    {

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTweening = true;
            float pos = isOpen ? -75 : 75;
            isOpen = !isOpen;
            transform.DOMoveX(pos, 0.5f).OnComplete(() =>
                {
                    isTweening = false;
                });
        }

        if (metaBuilding.gameObject.activeInHierarchy)
        {
            metaBuilding.transform.position = mouseWorldPos;
            if (Input.GetMouseButtonDown(0))
            {
                Building building = Instantiate(metaBuilding.building, metaBuilding.transform.position, Quaternion.identity, GameCenter.current.BuildingParent);
                building.gameObject.SetActive(true);
                metaBuilding.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                metaBuilding.gameObject.SetActive(false);
            }

        }
    }

    public void CreateBuildingMeta(Building prototype)
    {
        metaBuilding.image.sprite = prototype.spriteRenderer.sprite;
        metaBuilding.building = prototype;
        metaBuilding.gameObject.SetActive(true);
    }

}