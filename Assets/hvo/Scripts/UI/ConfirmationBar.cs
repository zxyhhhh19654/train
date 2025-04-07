using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ConfirmationBar : MonoBehaviour
{
    [SerializeField] private GameObject m_ConfirmationBar; //确认栏
    [SerializeField] private Button m_ConfirmationButton; //确认按钮
    [SerializeField] private Button m_CancelButton; //取消按钮

    public void ShowConfirmationBar()
    {
        gameObject.SetActive(true);
        
    }
    public void HideConfirmationBar()
    {
        gameObject.SetActive(false);
    }
    public void SetupHooks(UnityAction onConfirm, UnityAction onCancel)
    {
        m_ConfirmationButton.onClick.AddListener(onConfirm);
        m_CancelButton.onClick.AddListener(onCancel);
    }
    public void Disable()
    {
        m_ConfirmationButton.onClick.RemoveAllListeners();
        m_CancelButton.onClick.RemoveAllListeners();
    }
}