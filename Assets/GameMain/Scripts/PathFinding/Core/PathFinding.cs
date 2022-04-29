
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace PathFind
{
    public class PathFinding
    {
        public Grid<PathNode> grid;
        private const int UNIT_DIAGONAL_COST = 14;
        private const int UNIT_STRAIGHT_COST = 10;
        private Heap<PathNode> openSet;
        private List<PathNode> closeSet;

        public PathFinding(int width, int height, int cellSize = 10)
        {
            grid = new Grid<PathNode>(width, height, cellSize, (int x, int y) => new PathNode(x, y));
            UpdateGrid(width, height, cellSize);
        }


        public void UpdateGrid(int width, int height, int cellSize = 10)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Vector2 nodePos = new Vector2(x, y) * cellSize;
                    PathNode node = grid.GetValue(x, y);
                    node.grid = grid;
                    node.gCost = int.MaxValue;
                    node.isWalkable = GameCenter.current.CheckWalkable(x, y);
                    if (!node.isWalkable)
                        UnityEngine.Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y) + Vector3.down / 4, Color.red, 100f);
                    node.predecessor = null;
                }
            }
        }

        private bool CalculateWalkable(int x, int y, int cellSize)
        {
            bool hasObstacle = Physics2D.OverlapCircle(grid.GetWorldPosition(x, y), cellSize / 2, layerMask: LayerMask.NameToLayer("Building"));
            if (hasObstacle)
                UnityEngine.Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y) + Vector3.down / 4, Color.red, 100f);
            return !hasObstacle;
        }
        public void FindPath(Vector3 startPos, Vector3 endPos, out List<PathNode> path, out bool isSuccess)
        {
            grid.GetXY(startPos, out int startX, out int startY);
            grid.GetXY(endPos, out int endX, out int endY);
            FindPath(startX, startY, endX, endY, out List<PathNode> outPath, out bool outSuccess);
            path = outPath;
            isSuccess = outSuccess;
        }

        public List<Vector3> FindWayPoints(Vector3 startPos, Vector3 endPos, out bool success)
        {
            FindPath(startPos, endPos, out List<PathNode> path, out bool isSuccess);
            success = isSuccess;
            List<Vector3> waypoints = new List<Vector3>();
            if (path != null)
            {
                waypoints = SimplifyPath(path);
                waypoints.Add(endPos);
            }
            return waypoints;
        }

        public Vector3 FindNextPos(Vector3 startPos, Vector3 endPos)
        {
            grid.GetXY(startPos, out int startX, out int startY);
            grid.GetXY(endPos, out int endX, out int endY);
            FindPath(startX, startY, endX, endY, out List<PathNode> path, out bool isSuccess);
            if (!isSuccess)
                return endPos;
            PathNode nextNode = path[0];

            if (path.Count > 1)
                nextNode = path[1];

            if (nextNode == null)
                return startPos;
            else
                return grid.GetWorldPosition(nextNode.x, nextNode.y);
        }

        // Input: start and end position in grid map.
        // Output: The list of path node as the shortest path.
        public void FindPath(int startX, int startY, int endX, int endY, out List<PathNode> path, out bool isSuccess)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            PathNode startNode = grid.GetValue(startX, startY);
            PathNode endNode = grid.GetValue(endX, endY);

            openSet = new Heap<PathNode>(grid.Height * grid.Width);
            openSet.Add(startNode);
            closeSet = new List<PathNode>();

            startNode.gCost = 0;
            startNode.hCost = CalculateDisCost(startNode, endNode);

            while (openSet.Count > 0)
            {
                PathNode currentNode = openSet.RemoveFirst();

                if (currentNode == endNode)
                {
                    stopwatch.Start();
                    Debug.Log("Path Found in " + stopwatch.ElapsedMilliseconds + " ms.");
                    isSuccess = true;
                    path = CalculatePath(endNode);
                    return;
                }
                // openSet.Remove(currentNode);
                closeSet.Add(currentNode);

                foreach (var neighbourNode in currentNode.GetNeighbours())
                {
                    if (closeSet.Contains(neighbourNode) || !neighbourNode.isWalkable)
                        continue;
                    int gCost = currentNode.gCost + CalculateDisCost(currentNode, neighbourNode);
                    if (gCost < neighbourNode.gCost)
                    {
                        neighbourNode.predecessor = currentNode;
                        neighbourNode.gCost = gCost;
                        neighbourNode.hCost = CalculateDisCost(neighbourNode, endNode);

                        if (!openSet.Contains(neighbourNode))
                            openSet.Add(neighbourNode);
                    }
                }
            }
            path = null;
            isSuccess = false;
        }

        // Input: end Node.
        // Output: retraced path.
        // Retrace the path once the path finding is over.
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;

            while (currentNode.predecessor != null)
            {
                path.Add(currentNode.predecessor);
                currentNode = currentNode.predecessor;
            }
            path.Reverse();
            return path;
        }

        private List<Vector3> SimplifyPath(List<PathNode> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].x - path[i].x, path[i - 1].y - path[i].y);
                if (directionNew != directionOld)
                {
                    waypoints.Add(grid.GetWorldPosition(path[i].x, path[i].y));
                }
                directionOld = directionNew;
            }
            return waypoints;
        }

        // Input: current node n and end node
        // Output: h(n) cost
        // Calculate the heuristic cost by manhattan distance.
        private int CalculateDisCost(PathNode nodeA, PathNode nodeB)
        {
            int xDis = Mathf.Abs(nodeA.x - nodeB.x);
            int yDis = Mathf.Abs(nodeA.y - nodeB.y);
            int substract = Mathf.Abs(xDis - yDis);
            return UNIT_DIAGONAL_COST * Mathf.Min(xDis, yDis) + UNIT_STRAIGHT_COST * substract;
        }

        private PathNode GetLoswestFCostNode(List<PathNode> nodeList)
        {
            PathNode lowestNode = nodeList[0];
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].fCost < lowestNode.fCost)
                    lowestNode = nodeList[i];
            }
            return lowestNode;
        }

        public List<PathNode> VisualizePath(List<PathNode> nodes)
        {
            foreach (var node in nodes)
            {
                UnityEngine.Debug.DrawLine(grid.GetWorldPosition(node.x, node.y), grid.GetWorldPosition(node.x, node.y) + Vector3.down / 4, Color.green, 100f);
            }
            return nodes;
        }

    }
}
