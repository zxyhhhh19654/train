

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

//这是build的第一部分
public class placementProcess
{
    private GameObject m_PlacementOutline;//用来表示建筑对象

    private Vector3Int[] m_HeighlightPositions;//用来表示建筑对象瓦片
    private BuildAcitionSO m_BuildAcition;//这个是用来干嘛的？？？这个build的实现第一部分
    //大师，我悟了，这个m_BuildAcition是要新创建的tower的信息，将构造里传来的参数给这个。
    public Tilemap m_WalkableTileMap;

    private Tilemap m_OverlayTileMap;

    private Sprite m_PlaceholderTileSprite; //用来接受tower的照片

    private Tilemap[] m_UnreachableTilemaps;//用来放 不能放置物品 的层级。


    private Color m_HighlightColor = new Color(0,0.8f,1,0.4f);
    private Color m_BlockedColor = new Color(1f,0f,0,0.8f);
    public BuildAcitionSO BuildAcitionSO => m_BuildAcition;

    public int GoldCost => m_BuildAcition.GoldCost;
    public int FoodCost => m_BuildAcition.WoodCost;
    
    public placementProcess(
     BuildAcitionSO buildAcitionSO,
     Tilemap walkableTilemap,
    Tilemap overlayTileMap,
    Tilemap[] unreachableTilemaps
    )//建造前Tower显示
    {
        m_PlaceholderTileSprite = Resources.Load<Sprite>("Images/PlaceholderTileSprite");
        m_BuildAcition = buildAcitionSO;
        m_WalkableTileMap = walkableTilemap;
        m_OverlayTileMap = overlayTileMap;
        m_UnreachableTilemaps = unreachableTilemaps;
        //大师.......
    }

    public void Update()//
    {
        if(m_PlacementOutline != null)
        {
            HighlightTiles(m_PlacementOutline.transform.position);//实时显示tower的瓦片格子范围
        }
        if(HvoUtil.IsPointerOverUIElement()) return;//如果鼠标在UI上，则不执行下面的代码
        
        if(HvoUtil.TryGetHoldPosition(out Vector3 holdWorldPosition))
        {
            m_PlacementOutline.transform.position = SnapToGrid(holdWorldPosition);//实时确定tower阴影按照鼠标位置移动
        }
        
        // if(HvoUtil.TryGetHoldPosition(out Vector3 worldPositionInt))
        // {
        //     m_PlacementOutline.transform.position = SnapToGrid(worldPositionInt);//实时确定tower阴影按照鼠标位置移动
        // }
     }

    public void ShowPlacementOutline()//展示建造前的tower图片设置
    {
        
        m_PlacementOutline = new GameObject("PlacementOutline");//创建建造前的tower阴影
        var renderer = m_PlacementOutline.AddComponent<SpriteRenderer>();//增加组件
        renderer.sortingOrder = 999;//设置层数
        renderer.color = new Color(1, 1, 1, 0.3f);//tower阴影颜色
        var Sprite1 = m_BuildAcition.PlacementSprite;
        Debug.Log(Sprite1);
        renderer.sprite = Sprite1;//tower阴影添加照片
    }
    void ClearHightlight()
    {
        if(m_HeighlightPositions == null) return;
        foreach(var titlePosition in m_HeighlightPositions )
        {
            //Debug.Log("消除");
            m_OverlayTileMap.SetTile(titlePosition,null);
        }
    }
    public void CleanUp()
    {
        if(m_PlacementOutline != null)
        {
            GameObject.Destroy(m_PlacementOutline);//销毁建造前的tower阴影
            m_PlacementOutline = null;
        }
        ClearHightlight();//清除高亮化
    }
    public bool TryFinalizePosition(out Vector3 buildposition)//尝试确定建造位置
    {
        if(IsPlacementAreaValid())
        {
            ClearHightlight();
            buildposition = m_PlacementOutline.transform.position;//将建造前的tower阴影位置传递给buildposition
            UnityEngine.Object.Destroy(m_PlacementOutline);//销毁建造前的tower阴影
            //m_PlacementOutline.transform.position = Vector3.zero;//将建造前的tower阴影位置传递给buildposition
            return true;
        }
        Debug.Log("尝试确定建造位置");
        buildposition = Vector3.zero;//将建造前的tower阴影位置传递给buildposition
        return false;
    }
    bool IsPlacementAreaValid()//是否在可行走的层级中
    {
        foreach(var titlePosition in m_HeighlightPositions)//将建造范围的 每一个瓦片格子 都拿出来
        {
            if(!CanPlaceTile(titlePosition)) return false;//如果有一个瓦片格子不符合条件，则返回false
        }
        return true;//如果所有瓦片格子都符合条件，则返回true
       
    }
    bool IsInUnreachableTilemap(Vector3Int tilePosition)//是否在 不能行走层级中瓦片上
    {

        foreach (var tilemap in m_UnreachableTilemaps)//将不能行走的瓦片一个个拿出来
        {
            if (tilemap.HasTile(tilePosition)) return true;//检测有没有在 不能行走的瓦片上，如果在，返回true
        }
        return false;//检测有没有在 不能行走的瓦片上，如果没在，返回true
    }
    bool IsInreachableTilemap(Vector3Int tilePosition) => m_WalkableTileMap.HasTile(tilePosition);
    bool IsBlockedByGameobject(Vector3Int titlePosition) //是否在 不能行走层级中瓦片上
    {
        Vector3 tileSize = m_WalkableTileMap.cellSize;
        Collider2D[] colliders = Physics2D.OverlapBoxAll((titlePosition + tileSize/2),tileSize*0.3f,0);

        foreach (var collider in colliders)
        {
            if (collider != null)
            {
                var layer = collider.gameObject.layer;
                return true;
            }
        }
        return false;
    }
    bool CanPlaceTile(Vector3Int tilePosition)//是否在可行走的层级中的瓦片上
    {
        return IsInreachableTilemap(tilePosition)
        && !IsInUnreachableTilemap(tilePosition)
        && !IsBlockedByGameobject(tilePosition);

    }
       
    Vector3 SnapToGrid(Vector3 worldPosition)//这个是为了将坐标取整化，利用FloorToInt这个方法四舍五入
    {
        return new Vector3(Mathf.FloorToInt(worldPosition.x),Mathf.FloorToInt(worldPosition.y),0);
    }

    void HighlightTiles(Vector3 outlinePosition)//建造范围高亮化
    {
        Vector3Int buildingSize = m_BuildAcition.BuildingSize;//将建造范围的x,y值存储到数据中
        Vector3 pivotPosition = outlinePosition + m_BuildAcition.OriginOffset;
        ClearHightlight();
        m_HeighlightPositions = new Vector3Int[buildingSize.x * buildingSize.y];//创建一个 和tower瓦片数量大小 的数组

        for(int x = 0; x < buildingSize.x; x++)
        {
            for (int y = 0; y < buildingSize.y; y++)
            {
                m_HeighlightPositions[x + y*buildingSize.x] = new Vector3Int((int)pivotPosition.x + x,(int)pivotPosition.y+y,0);
                //将建造范围的 每一瓦片格子 都存储在 m_HeighlightPositions中
            }
        }
    

        foreach( var titlePosition in m_HeighlightPositions)//将建造范围的 每一个瓦片格子 都拿出来
        {
            var tile = ScriptableObject.CreateInstance<Tile>();// 创建一个新的 Tile 实例
            tile.sprite = m_PlaceholderTileSprite;//必须先要将tile的sprite赋值后，才能下面的，由顺序要求
            if(CanPlaceTile(titlePosition))
            {
                tile.color = m_HighlightColor;
            }
            else
            {
                tile.color = m_BlockedColor;
            }
            m_OverlayTileMap.SetTile(titlePosition,tile);// 在指定位置设置瓦片
        }

    // }
    
    
    
    // //检测有没有在 不能行走的瓦片上，如果在，返回true，反之，则返回false
    

    

    }
}