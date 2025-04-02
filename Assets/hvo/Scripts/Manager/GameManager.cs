
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : SigleManager<GameManager>  
{
    [Header("游戏状态")]
    [SerializeField] private Unit m_ActvieUnit; //当前选中的单位
    [Header("UI")]
    [SerializeField] private GameObject m_ClickToPointPrefab;//点击地面特效预制体
  

    Vector2 tmp;
    void Update()
    {
        if(HvoUtil.TryGetShortLiftClickPosition(out Vector2 inputPosition))
        {
            
            DetectClick(inputPosition);
        }
    }

    void DetectClick(Vector2 inputPosition)
    {
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
        if(m_ActvieUnit == null)
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

   bool HasCilckOnHuman(Unit unit) => m_ActvieUnit != null ;

    void HandOnUnit(Unit unit)
    {
        if (HasCilckOnHuman(unit))
        {
            CancelUnit();
        }

        SelectNewUnit(unit);
    }
    void SelectNewUnit(Unit unit)
    {
        Debug.Log("变更单位");
        m_ActvieUnit = unit;
        m_ActvieUnit.Select();
    }
    void CancelUnit()//取消unit
    {
        m_ActvieUnit.UnSelect();
        m_ActvieUnit = null;//将activeunit置为空
        
    }

    void DispalyClickEffect(Vector2 worldpoistion)//显示鼠标
    {
        Debug.Log("进入鼠标复制页面");
        Instantiate(m_ClickToPointPrefab, worldpoistion, Quaternion.identity);
        Debug.Log("鼠标复制结束");
    }

}
    



