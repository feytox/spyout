using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public Tilemap[] ObstacleTilemaps;
    public GameObject TestStart;
    public GameObject TestEnd;
    
    private TileGrid tileGrid;

    void Start()
    {
        RefreshGrid();
        Debug.Log(tileGrid);
        
        Debug.Log(TestStart.transform.position);
        
        var start = ObstacleTilemaps[0].WorldToCell(TestStart.transform.position);
        var end = ObstacleTilemaps[0].WorldToCell(TestEnd.transform.position);
        
        foreach (var pos in PathFinder.FindAStarPath(tileGrid, start, end))
        {
            Debug.Log(pos);
        }    
    }

    private void RefreshGrid() // maybe add refreshing after tile change
    {
        tileGrid = TileGrid.Parse(ObstacleTilemaps);
    }
}