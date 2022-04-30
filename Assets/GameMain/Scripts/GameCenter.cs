using UnityEngine;
using System.Collections.Generic;
using GameKit;
public class GameCenter : MonoSingletonBase<GameCenter>
{
    public static bool TIME_ACTIVE = true;
    public static int TIME_MULTIPILER = 1;
    [Range(30, 60)] public float timePerDay = 60;
    public int currentDay = 1;
    public float currentTime = 0;
    public List<Vector2Int> unwalkblePos = new List<Vector2Int>();
    public Dictionary<BuildingHelperType, List<Building>> buildings = new Dictionary<BuildingHelperType, List<Building>>();
    public bool IsMorning
    {
        get { return currentTime < 20f; }
    }
    public bool IsNoon
    {
        get { return currentTime >= 20f && currentTime < 40; }
    }

    public bool IsNight
    {
        get { return currentTime >= 40f && currentTime < 60f; }
    }
    public void RegisterUnwalkable(Vector2Int pos)
    {
        if (!unwalkblePos.Contains(pos))
        {
            unwalkblePos.Add(pos);
        }
    }
    public void RemoveUnwalkable(Vector2Int pos)
    {
        if (unwalkblePos.Contains(pos))
        {
            unwalkblePos.Remove(pos);
        }
    }

    public void RegisterBuilding(BuildingHelperType type, Building buiding)
    {
        if (!buildings.ContainsKey(type))
            buildings.Add(type, new List<Building>());
        buildings[type].Add(buiding);
    }

    public bool CheckWalkable(Vector2Int pos)
    {
        return !unwalkblePos.Contains(pos);
    }
    public bool CheckWalkable(int x, int y)
    {
        return CheckWalkable(new Vector2Int(x, y));
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timePerDay)
        {
            currentTime = 0;
            currentDay++;
        }
    }
}