using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using PathFind;
using GameKit;

public class PathFindingManager : MonoSingletonBase<PathFindingManager>
{

    public Vector2Int mapSize = new Vector2Int(50, 50);
    public int cellSize = 1;
    public Grid<PathNode> grid;
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;
    private PathFinding pathfinding;
    private bool isProcessing = false;

    protected override void OnAwake()
    {
        pathfinding = new PathFinding(mapSize.x, mapSize.y, cellSize);
        grid = pathfinding.grid;
    }

    private void Update()
    {

        if (pathRequestQueue.Count > 0)
        {
            UpdatePathFindingGrid();
            currentPathRequest = pathRequestQueue.Dequeue();
            List<Vector3> waypoints = pathfinding.FindWayPoints(currentPathRequest.pathStart, currentPathRequest.pathEnd, out bool success);
            currentPathRequest.callback.Invoke(waypoints, success);
        }
    }

    public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<List<Vector3>, bool> callback)
    {
        // Debug.Log(pathStart + " To " + pathEnd);
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        pathRequestQueue.Enqueue(newRequest);
    }
    public class PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<List<Vector3>, bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<List<Vector3>, bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

    public void UpdatePathFindingGrid() => pathfinding.UpdateGrid(mapSize.x, mapSize.y, cellSize);
}