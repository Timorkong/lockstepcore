using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum EnumTileType
{
    Ground,
    Wall,
}

public class LevelManager : MonoSingleton<LevelManager>
{
    protected Tilemap[] tilemaps = null;

    protected Tilemap mainTileMap = null;

    public Vector3 vecInfo = new Vector3();

    protected override void Awake()
    {
        base.Awake();

        Init();
    }
    [ContextMenu("获取信息")]
    public void GetInfo()
    {
        Init();

        var pos = new FixedVec3();
        pos.x.SetFloat(vecInfo.x);
        pos.y.SetFloat(vecInfo.y);
        pos.z.SetFloat(vecInfo.z);

        GetTileType(pos);
    }

    public void Init()
    {
        tilemaps = gameObject.GetComponentsInChildren<Tilemap>();

        mainTileMap = tilemaps[0];
    }

    public EnumTileType GetTileType(FixedVec3 pos)
    {
        var ret = EnumTileType.Ground;

        var vIntPos = GetTilePos(pos);

        var tile = mainTileMap.GetTile<Tile>(vIntPos);

        switch (tile.colliderType)
        {
            case Tile.ColliderType.None:
                ret = EnumTileType.Ground;
                break;
            default:
                ret = EnumTileType.Wall;
                break;
        }

        return ret;
    }

    public Vector3Int GetTilePos(FixedVec3 pos)
    {
        var ret = new Vector3Int();

        var tmp = FixedVec3.GetPool().CopyFrom(pos);

        var size = FixedVec3.GetPool().SetFloat(mainTileMap.cellSize.x, mainTileMap.cellSize.y, mainTileMap.cellSize.z);

        ret.x = Mathf.FloorToInt((tmp.x / size.x).rawVal);

        ret.y = Mathf.FloorToInt((tmp.y / size.y).rawVal);

        tmp.PoolRecover();

        size.PoolRecover();

        return ret;
    }
}
