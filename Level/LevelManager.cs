using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Ground,
    Wall,
}

public class LevelManager : MonoSingleton<LevelManager>
{
    protected Tilemap[] tileMaps = null;

    protected Tilemap mainTileMap = null;

    public void Start()
    {
        InitMap();
    }

    public void InitMap()
    {
        tileMaps = gameObject.GetComponentsInChildren<Tilemap>();

        mainTileMap = tileMaps[0];

        foreach(var map in tileMaps)
        {
            map.CompressBounds();
        }
    }

    public void GetCurTypeByPos(FixedVec3 pos)
    {

    }
}
