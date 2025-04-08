
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
    [SerializeField] private ConfirmationBar m_BuildConfirmationBar;//建造确认栏


    private int m_Gold = 1500;//金币数量
    public int Gold => m_Gold;//金币数量的getter

    private int m_Wood = 1500;//食物数量
    public int Wood => m_Wood;//食物数量的getter


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
            if (HvoUtil.TryGetShortLeftClickPosition(out Vector2 inputPosition))
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
        if (m_PlacementProcess != null)
        {
            return;
        }
        m_PlacementProcess = new placementProcess(
            buildAcition,
            m_WalkableTileMap,
            m_OverlayTileMap,
            m_UnreachableTilemaps
        );//建造过程
        m_PlacementProcess.ShowPlacementOutline();//展示建造前的tower图片设置
        m_BuildConfirmationBar.ShowConfirmationBar();
        m_BuildConfirmationBar.ShowResourceRequirements(buildAcition.GoldCost, buildAcition.WoodCost);//展示建造所需资源
        m_BuildConfirmationBar.SetupHooks(ConfirmBuildPlace, CancelBuildPlace);//设置确认和取消按钮的回调函数
        //Debug.Log("开始建造过程" + buildAcition.ActionName);

    }

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

        RaycastHit2D hit2D = Physics2D.Raycast(worldPosition, Vector2.zero);



        if (HsaClickOnUnit(hit2D, out var unit))
        {

            HandOnUnit(unit);
        }
        else
        {

            HandOnGrand(worldPosition);
        }

    }

    void HandOnGrand(Vector2 worldPosition)
    {
        if (m_ActvieUnit == null)
        {
            return;
        }
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

        Instantiate(m_ClickToPointPrefab, worldpoistion, Quaternion.identity);

    }
    void ConfirmBuildPlace()
    {
        if (m_PlacementProcess == null)
        {
           // Debug.LogError("m_PlacementProcess is null. Cannot confirm build placement.");
            m_BuildConfirmationBar.HideConfirmationBar();
            return;
        }
        var currentProcess = m_PlacementProcess;
        int goldCost = currentProcess.GoldCost;
        int woodCost = currentProcess.FoodCost;
        if (!TryDeductResource(goldCost, woodCost))//尝试扣除资源
        {
            //Debug.Log("资源不足，无法建造！");
            return;
        }
        if (m_PlacementProcess.TryFinalizePosition(out Vector3 buildPosition))//尝试确定建造位置
        {
            m_BuildConfirmationBar.HideConfirmationBar();

            new BuildingProcess(
                currentProcess.BuildAcitionSO,
                buildPosition
            );
            m_ActvieUnit.MoveTo(buildPosition);//选择目的地
            //Debug.Log("建造位置合法！");
            m_PlacementProcess = null;

        }
        else
        {

            RevertResource(goldCost, woodCost);
            currentProcess.CleanUp();
            m_PlacementProcess = null;

        }
    }

    void RevertResource(int goldCost, int woodCost)//还原资源
    {
        m_Gold += goldCost;//还原金币
        m_Wood += woodCost;//还原食物

    }
    void CancelBuildPlace()
    {
        if (m_PlacementProcess != null)
        {
            m_PlacementProcess.CleanUp();
            m_PlacementProcess = null;
        }
        m_BuildConfirmationBar.HideConfirmationBar();
    }

    bool TryDeductResource(int goldCost, int woodCost)//尝试扣除资源
    {
        if (m_Gold >= goldCost && m_Wood >= woodCost)//如果金币和食物都足够
        {
            m_Gold -= goldCost;//扣除金币
            m_Wood -= woodCost;//扣除食物
            return true;

        }
        return false;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 40, 200, 20), "Gold:" + m_Gold.ToString(), new GUIStyle { fontSize = 30 });//显示金币数量
        GUI.Label(new Rect(10, 80, 200, 20), "Wood:" + m_Wood.ToString(), new GUIStyle { fontSize = 30 });//显示金币数量
    }

}




