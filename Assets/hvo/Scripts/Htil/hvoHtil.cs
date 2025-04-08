

using UnityEngine;
using UnityEngine.EventSystems;

public static class HvoUtil     //这个类是一个工具类，主要用于处理输入事件和UI交互
{
    public static Vector2 InputPosition => Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;

    public static bool IsleftClickOrTapDawn => Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

    public static bool IsleftClickOrTapUp => Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);




    private static Vector2 m_InitialTouchPosition;  //初始点击位置
    private static bool m_HasInitialPositionSet = false; //这个变量是用来判断初始点击位置是否已经设置的，主要是为了处理短按和长按事件
    //这个变量是用来保存初始点击位置的，主要是为了处理短按和长按事件

    public static bool TryGetShortLeftClickPosition(out Vector2 inputPosition, float maxDistance = 5f)
    {
            inputPosition = InputPosition;

            if (HvoUtil.IsleftClickOrTapDawn)
            {
                //Debug.Log("保存位置");
                m_HasInitialPositionSet = true; // 标记初始位置已设置
                m_InitialTouchPosition = InputPosition;//这个只是将初始位置保存,用来与离开时手指位置进行比较
                return false; // 返回false，表示没有检测到短按
            }
            if (IsleftClickOrTapUp)
            {
                if (Vector2.Distance(m_InitialTouchPosition, InputPosition) < maxDistance)
                {
                    return true;
                }
            }
            if (m_HasInitialPositionSet && IsleftClickOrTapUp)
            {
                m_HasInitialPositionSet = false; // 重置初始位置标记
            }
            inputPosition = Vector2.zero;
            return false;
    }

    public static bool TryGetHoldPosition(out Vector3 worldPosition)        //获取点击位置
    //这个函数是用来获取点击位置的，主要是为了处理触摸和鼠标点击事件
    {
        if(Input.touchCount > 0)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position );
            return true;
        }
        else if(Input.GetMouseButton(0))
        {
            worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition );
            return true;
        }
        else
        {
            worldPosition = Vector3.zero;
            return false;
        }
    }
    public static bool IsPointerOverUIElement()//是否点击到ui按钮。
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
    
}