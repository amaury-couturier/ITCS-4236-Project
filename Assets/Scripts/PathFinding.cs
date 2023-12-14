using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;

    GridController grid;

    void Awake()
    {
        grid = GetComponent<GridController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            FindPath(seeker.position, target.position);
        }
    }

    public void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Tile startTile = grid.TileFromWorldPoint(startPos);
        Tile targetTile = grid.TileFromWorldPoint(targetPos);
        
        Heap<Tile> openSet = new Heap<Tile>(grid.MaxSize);
        HashSet<Tile> closedSet = new HashSet<Tile>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tile currentTile = openSet.RemoveFirst();
            closedSet.Add(currentTile);

            if (currentTile == targetTile)
            {   
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + "ms");

                RetracePath(startTile, targetTile);
                return;
            }

            foreach (Tile neighbor in grid.GetNeighbors(currentTile))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentTile.gCost + GetDistance(currentTile, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetTile);
                    neighbor.parent = currentTile;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Tile tileA, Tile tileB)
    {
        int distanceX = Mathf.Abs(tileA.gridX - tileB.gridX);
        int distanceY = Mathf.Abs(tileA.gridY - tileB.gridY);

        if (distanceX > distanceY)
        {
            return 14*distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
