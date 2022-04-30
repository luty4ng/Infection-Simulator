using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFind
{
    public class PathNode : IHeap<PathNode>
    {
        public int x;
        public int y;
        public Grid<PathNode> grid;

        public int gCost;
        public int hCost;
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
        public bool isWalkable = true;
        private int heapIndex;
        public PathNode predecessor;
        public PathNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return x + "," + y;
        }

        public int index
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(PathNode nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
                compare = hCost.CompareTo(nodeToCompare.hCost);
            return -compare;
        }
        public List<PathNode> GetNeighbours()
        {
            List<PathNode> neighbours = new List<PathNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int X = this.x + x;
                    int Y = this.y + y;

                    if (X >= 0 && Y < grid.Width && Y >= 0 && Y < grid.Height)
                        neighbours.Add(grid.GetValue(X, Y));
                }
            }

            return neighbours;
        }
    }

}
