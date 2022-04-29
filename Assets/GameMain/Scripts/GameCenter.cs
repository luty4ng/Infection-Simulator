using UnityEngine;
using System.Collections.Generic;
using GameKit;
public class GameCenter : MonoSingletonBase<GameCenter>
{
    public static bool TIME_ACTIVE = true;
    public static int TIME_MULTIPILER = 1;
    [Range(30, 60)] public float timePerDay = 30;
    public int currentDay = 1;
    public float currentTime = 0;
    public List<Vector2Int> unwalkblePos = new List<Vector2Int>();

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

    public bool CheckWalkable(Vector2Int pos)
    {
        return !unwalkblePos.Contains(pos);
    }
    public bool CheckWalkable(int x, int y)
    {
        return CheckWalkable(new Vector2Int(x, y));
    }

}