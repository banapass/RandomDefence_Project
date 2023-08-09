using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    private Node startNode;
    private Node endNode;
    private Node[,] grid;
    private int gridSizeX, gridSizeY;
    private List<Vector3> path;
    public List<Vector3> Path { get { return path; } }

    public void Init()
    {
        Vector2Int mapSize = Constants.MAP_SIZE;
        gridSizeX = mapSize.x;
        gridSizeY = mapSize.y;
        grid = new Node[gridSizeY, gridSizeX];
        InitializeGrid();
        SetStartEndPoint();
    }
    private void SetStartEndPoint()
    {
        startNode = grid[0, 0];
        endNode = grid[gridSizeY - 1, gridSizeX - 1];
    }
    public void InitializeGrid(BaseTile[,] _tilemap)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                BaseTile _currTile = _tilemap[y, x];
                Vector3 worldPosition = _currTile.transform.position;
                grid[y, x] = new Node(_currTile.IsWalkable, worldPosition, x, y);
            }
        }
    }
    public void SynchronizeAt(BaseTile[,] _tilemap, bool _updateWalkable = false, bool _updateWorldPos = false)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Node _node = grid[y, x];
                BaseTile _currTile = _tilemap[y, x];
                if (_updateWalkable) _node.walkable = _currTile.IsWalkable;
                if (_updateWorldPos) _node.worldPosition = _currTile.transform.position;
            }
        }
    }
    private void InitializeGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
                grid[y, x] = new Node(true, default, x, y);
    }

    public bool IsCanPlaceTile(EmptyTile _tile)
    {
        Queue<Node> _openSet = new Queue<Node>();
        List<Node> _visited = new List<Node>();
        Node _targetNode = grid[_tile.TileCoord.y, _tile.TileCoord.x];

        _openSet.Enqueue(startNode);
        _targetNode.walkable = false;

        while (_openSet.Count > 0)
        {
            Node _currentNode = _openSet.Dequeue();

            if (_currentNode == endNode) return true;

            foreach (Node _node in GetNeighbors(_currentNode))
            {
                if (_visited.Contains(_node)) continue;
                if (!_node.walkable) continue;

                if (!_visited.Contains(_node))
                    _visited.Add(_node);

                _openSet.Enqueue(_node);
            }
        }

        _targetNode.walkable = true;

        return false;
    }
    public List<Vector3> FindPath()
    {
        if (path == null) path = new List<Vector3>();
        else path.Clear();



        if (startNode == null || endNode == null)
        {
            SetStartEndPoint();
            Debug.LogError("Start or end node is null!");
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
                if (openSet[i].fCost < currentNode.fCost ||
                   (openSet[i].fCost == currentNode.fCost &&
                    openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            path.Add(currentNode.worldPosition);
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // 목표 노드 도착
            if (currentNode == endNode)
                return path;


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
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    // neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    // List<Vector3> RetracePath(Node startNode, Node endNode)
    // {
    //     List<Vector3> path = new List<Vector3>();
    //     Node currentNode = endNode;

    //     while (currentNode != startNode)
    //     {
    //         path.Add(currentNode.worldPosition);
    //         currentNode = currentNode.parent;
    //     }
    //     path.Reverse();

    //     return path;
    // }

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

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return distX + distY;
    }

}