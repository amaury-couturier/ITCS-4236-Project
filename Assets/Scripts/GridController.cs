using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController: MonoBehaviour
{
    public bool onlyDisplayPathGizmos;
    public Transform guard;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float tileRadius;
    Tile[,] grid;

    private float tileDiameter;
    private int gridSizeX, gridSizeY;

    void Start()
    {
        tileDiameter = tileRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / tileDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / tileDiameter);
        CreateGrid();
    }

    public int MaxSize 
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Tile[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * tileDiameter + tileRadius) + Vector3.up * ( y * tileDiameter + tileRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, tileRadius, unwalkableMask));
                grid[x,y] = new Tile(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = tile.gridX + x;
                int checkY = tile.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Tile TileFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x,y];
    }

    public List<Tile> path;
    void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach (Tile t in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(t.worldPosition, Vector3.one * (tileDiameter - .1f));
                }
            }
        }
        else
        {
            if(grid != null)
            {
                Tile guardTile = TileFromWorldPoint(guard.position);
                foreach (Tile t in grid)
                {
                    Gizmos.color = (t.walkable) ? Color.white : Color.red;
                    if (guardTile == t)
                    {
                        Gizmos.color = Color.green;
                    }
                    if (path != null)
                        if (path.Contains(t))
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(t.worldPosition, Vector3.one * (tileDiameter - .1f));
                }
            }
        }
    }
}
