using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    private Node[,] grid;
    private int gridSizeX, gridSizeY;

    public void Init()
    {
        Vector2Int mapSize = Constants.MAP_SIZE;
        gridSizeX = mapSize.x;
        gridSizeY = mapSize.y;
        grid = new Node[gridSizeY, gridSizeX];
    }

    public void InitializeGrid(Tile[,] _tilemap)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Tile _currTile = _tilemap[y, x];
                Vector3 worldPosition = _currTile.transform.position;
                grid[y, x] = new Node(_currTile.IsWalkable, worldPosition, x, y);
            }
        }
    }

    public List<Vector3> FindPath()
    {
        List<Vector3> path = new List<Vector3>();

        Node startNode = grid[0, 0];
        Node targetNode = grid[gridSizeY - 1, gridSizeX - 1];

        if (startNode == null || targetNode == null)
        {
            Debug.LogError("Start or target node is null!");
            return path;
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                path = RetracePath(startNode, targetNode);
                return path;
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return path;
    }

    List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        // int[] offsets = { -1, 0, 1 };
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                // if (x == 0 && y == 0)
                //     continue;
                if (x == 0 || y == 0)
                {
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbors.Add(grid[checkY, checkX]);
                    }
                }

            }
        }
        // foreach (int x in offsets)
        // {
        //     foreach (int y in offsets)
        //     {
        //         if (x == 0 && y == 0)
        //             continue;

        //         int checkX = node.gridX + x;
        //         int checkY = node.gridY + y;

        //         if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        //         {
        //             neighbors.Add(grid[checkX, checkY]);
        //         }
        //     }
        // }
        return neighbors;
    }

    // 월드 포지션에 해당하는 노드 가져오기
    Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x);
        int y = Mathf.FloorToInt(worldPosition.y);

        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            return grid[y, x];
        }
        return null;
    }

    // 두 노드 사이의 거리 계산
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return distX + distY;
    }

    // 그리드 노드 클래스
    public class Node
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX, gridY;
        public int gCost, hCost;
        public Node parent;

        public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }
    }
}