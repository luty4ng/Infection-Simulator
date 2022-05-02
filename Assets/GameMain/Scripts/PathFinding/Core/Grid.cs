using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PathFind
{
    public class Grid<TGrid>
    {
        private readonly int width;
        private readonly int height;
        private readonly float cellSize;
        private readonly float offset;
        private readonly Vector2 pivot;
        private TGrid[,] gridMap;

        public int Height
        {
            get
            {
                return height;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public Grid(int width, int height, float cellSize = 10f, System.Func<int, int, TGrid> initialize = null)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.offset = cellSize / 2;
            this.pivot = new Vector2(width / 2, height / 2);
            this.gridMap = new TGrid[width, height];

            for (int x = 0; x < gridMap.GetLength(0); x++)
            {
                for (int y = 0; y < gridMap.GetLength(1); y++)
                {
                    if (initialize != null)
                        gridMap[x, y] = initialize.Invoke(x, y);
                    // debugMap[x, y] = DebugUtilities.GridVisual(gridMap[x, y].ToString(), GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, Color.white, fontSize: 5);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            Debug.Log("Creating Grid. Width: " + width + " Height: " + height);
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x - pivot.x + offset, y - pivot.y + offset, 0) * cellSize;
        }
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition.x + pivot.x - offset) / cellSize);
            y = Mathf.FloorToInt((worldPosition.y + pivot.y - offset) / cellSize);
        }

        public void SetValue(int x, int y, TGrid value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridMap[x, y] = value;
            }
        }

        public void SetValue(Vector3 worldPosition, TGrid value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetValue(x, y, value);
        }

        public TGrid GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                return gridMap[x, y];
            else
                return default(TGrid);
        }

        public TGrid GetValue(Vector3 worldPosition, TGrid value)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetValue(x, y);
        }

        public Vector2 Clamp(Vector2 worldPos)
        {
            GetXY(worldPos, out int x, out int y);
            if (x < 0)
                x = 3;
            else if (x > width)
                x = width - 3;
            if (y < 0)
                y = 3;
            else if (y > height)
                y = height - 3;

            return GetWorldPosition(x, y);
        }
    }
}

