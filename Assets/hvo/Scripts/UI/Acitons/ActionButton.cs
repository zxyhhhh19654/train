

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ACtionButton : MonoBehaviour   //这个类是一个UI组件，用于显示和管理游戏中的动作按钮
// 这个类是一个UI组件，用于显示和管理游戏中的动作按钮
{
    [SerializeField] private Image m_IconImage;//icon是图标（南瓜头）
    [SerializeField] private Button m_Button;//button是地板

    void OnDestroy()
    {
        m_Button.onClick.RemoveAllListeners();
    }
    public void InitIcon(Sprite icon,UnityAction unityAction)
    {
        m_IconImage.sprite = icon;
        m_Button.onClick.AddListener(unityAction);
    }
}