
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour  //动作按钮
{
    [SerializeField]private Image m_GroundImage;
    [SerializeField] private ACtionButton m_ActionButtonPrefab;

   private Color m_OriColor;

   private List<ACtionButton> m_ActionButtions = new(); //一个列表用来存储按钮（图标）

    void Awake()    
    {
        m_OriColor = m_GroundImage.color;//将地板最开始的颜色保存起来
    }
    public void RegisterAction(Sprite icon,UnityAction unityAction)//复制一个按钮到它的子路径
    {
        var actionButton = Instantiate(m_ActionButtonPrefab,transform);
        actionButton.InitIcon(icon,unityAction);
        m_ActionButtions.Add(actionButton);//将它添加到数组中
    }

    public void ClearAction()//删除所有的按钮
    {
        for(int i = m_ActionButtions.Count - 1; i >= 0; i--)
        {
            Destroy(m_ActionButtions[i].gameObject);
            m_ActionButtions.RemoveAt(i);
        }
    }

    public void Show()//将地板展示
    {
        m_GroundImage.color = m_OriColor;
    }
    public void Hide()//将地板隐藏
    {
        m_GroundImage.color = new Color(0,0,0,0);
    }

}