
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : SigleManager<GameManager>
{
    [Header("Tilemap")]
    [SerializeField] private Tilemap m_WalkableTileMap;//可行走的瓦片地图
    [SerializeField] private Tilemap m_OverlayTileMap;//覆盖的瓦片地图
    [SerializeField] private Tilemap[] m_UnreachableTilemaps;//不可行走的瓦片地图

    [Header("游戏状态")]
    [SerializeField] private Unit m_ActvieUnit; //当前选中的单位

    private placementProcess m_PlacementProcess;//建造过程
    [Header("UI")]
    [SerializeField] private GameObject m_ClickToPointPrefab;//点击地面特效预制体
    [SerializeField] private ActionBar m_ActionBar;//动作按钮


    void Start()
    {
        ClearActionBarUI();//开始时清除ui
        m_ActionBar.Hide();//开始时隐藏地板
    }
    Vector2 tmp;
    void Update()
    {
        if (m_PlacementProcess != null)
        {
            m_PlacementProcess.Update();//更新建造过程
        }
        else
        {
            if (HvoUtil.TryGetShortLiftClickPosition(out Vector2 inputPosition))
            {
                DetectClick(inputPosition);
            }
        }

    }

    void ClearActionBarUI()//消除所有ui
    {
        m_ActionBar.ClearAction();//删除按钮后，直接消失

        m_ActionBar.Hide();//隐藏地板
    }
    void ShowUnitAntions(Unit unit)//展示ui的手段：地板转化颜色，创建按钮。
    {
        ClearActionBarUI();//在展示新ui之前要删除之前的ui，要不然会ui会叠加
        if (unit.Actions.Length == 0) return;
        m_ActionBar.Show();

        foreach (var action in unit.Actions)
        {
            m_ActionBar.RegisterAction(action.Icon//创建技能按钮
            , () => action.Execute(this)
            );
        }
    }
    public void StartBuildProcess(BuildAcitionSO buildAcition)
    {
        m_PlacementProcess = new placementProcess(
            buildAcition,
            m_WalkableTileMap,
            m_OverlayTileMap,
            m_UnreachableTilemaps
        );//建造过程
        m_PlacementProcess.ShowPlacementOutline();//展示建造前的tower图片设置
        Debug.Log("开始建造过程" + buildAcition.ActionName);
        
    }

    // public void StartBuildProcess(BuildAcitionSO buildAcition)//建造过程
    // {

    //     Debug.Log("开始建造过程" + buildAcition.ActionName);
    //     if(m_PlacementProcess != null)
    //     {
    //         return;//清除之前的地表位置
    //     }
    //     Debug.Log("Starting action" + buildAcition.ActionName);
    //     m_PlacementProcess = new placementProcess(
    //         buildAcition,
    //         m_WalkableTileMap,
    //         m_OverlayTileMap,
    //         m_UnreachableTilemaps
    //     );//为什么这里不用局部变量
    //     因为这个变量在update（）还要调用
    //     m_PlacementProcess.ShowPlacementOutline();//查看显示地表位置
    //    m_BuildConfirmationBar.SetupHooks(ConfirmBuildPlacement, CancelBuildPlacement);//设置确认和取消按钮的回调函数
    // }

    void DetectClick(Vector2 inputPosition)
    {

        if (HvoUtil.IsPointerOverUIElement())//如果点击到ui按钮，直接返回
        {
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("Main Camera is not assigned!");
            return;
        }
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
        Debug.Log("世界坐标");
        RaycastHit2D hit2D = Physics2D.Raycast(worldPosition, Vector2.zero);
        Debug.Log("射线");



        if (HsaClickOnUnit(hit2D, out var unit))
        {
            Debug.Log("选择单位");
            HandOnUnit(unit);
        }
        else
        {
            Debug.Log("选择地形");
            HandOnGrand(worldPosition);
        }

    }

    void HandOnGrand(Vector2 worldPosition)
    {
        if (m_ActvieUnit == null)
        {
            Debug.Log("没有选择单位");
            return;
        }

        Debug.Log("选择地形行动");

        DispalyClickEffect(worldPosition);//显示鼠标特效
        m_ActvieUnit.MoveTo(worldPosition);//选择目的地
    }
    bool HsaClickOnUnit(RaycastHit2D hit2D, out Unit unit)
    {

        if (hit2D.collider != null && hit2D.collider.TryGetComponent<Unit>(out var clickedUnit))
        {
            unit = clickedUnit;
            return true;
        }
        unit = null;
        return false;
    }

    bool HasCilckOnHuman(Unit unit) => m_ActvieUnit != null;

    void HandOnUnit(Unit unit)
    {
        if (HasCilckOnHuman(unit))
        {
            CancelUnit();
            return;
        }

        SelectNewUnit(unit);
    }
    void SelectNewUnit(Unit unit)
    {
        Debug.Log("变更单位");
        m_ActvieUnit = unit;
        m_ActvieUnit.Select();
        ShowUnitAntions(unit);//展示ui
    }
    void CancelUnit()//取消unit
    {
        m_ActvieUnit.UnSelect();

        m_ActionBar.ClearAction();//删除按钮后，直接消失
        m_ActionBar.Hide();//隐藏地板

        m_ActvieUnit = null;//将activeunit置为空


    }

    void DispalyClickEffect(Vector2 worldpoistion)//显示鼠标
    {
        Debug.Log("进入鼠标复制页面");
        Instantiate(m_ClickToPointPrefab, worldpoistion, Quaternion.identity);
        Debug.Log("鼠标复制结束");
    }

}




