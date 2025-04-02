
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : SigleManager<GameManager>  
{
    [Header("游戏状态")]
    [SerializeField] private Unit m_ActvieUnit; //当前选中的单位

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
    
   bool HasCilckOnHuman(Unit unit) => m_ActvieUnit != null && m_ActvieUnit == unit;

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
    }
    void CancelUnit()//取消unit
    {
        m_ActvieUnit = null;//将activeunit置为空
    }

}
    



