using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : IHeapItem<Tile>
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Tile parent;
    int heapIndex;

    public Tile(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get 
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex{
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo (Tile tileToComapre)
    {
        int compare = fCost.CompareTo(tileToComapre.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(tileToComapre.hCost);
        }
        return -compare;
    }
}
