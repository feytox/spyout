using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public Tilemap[] ObstacleTilemaps;
    private Grid grid;

    void Start()
    {
        RefreshGrid();
        Debug.Log(grid);
    }

    private void RefreshGrid() // maybe add refreshing after tile change
    {
        grid = Grid.Parse(ObstacleTilemaps);
    }
}