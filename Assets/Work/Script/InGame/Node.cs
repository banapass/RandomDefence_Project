using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    //public int gridX, gridY;
    public Coord coord;
    public int gCost, hCost;
    public Node parent;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        //this.gridX = gridX;
        //this.gridY = gridY;
        coord = new Coord(gridX, gridY);
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }
}