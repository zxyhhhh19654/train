
using TMPro;
using UnityEngine;

public class ResourceRequmentsDispaly : MonoBehaviour
{
    [Header("资源需求显示")]
    [SerializeField] private TextMeshProUGUI m_GoldText;
    [SerializeField] private TextMeshProUGUI m_WoodText;

    //[SerializeField] private GameManager gameManager; //资源需求显示
    void UpadtaColorRequirements(int reqGold, int reqWood)
    {
        var manager = GameManager.Get();
        var greenColor = new Color(0f, 0.7f, 1f, 1f);

        m_GoldText.color = manager.Gold >= reqGold ? Color.green : Color.red;
        m_WoodText.color = manager.Wood >= reqWood ? Color.green : Color.red;
    }
    public void ShowResourceRequirements(int reqGold, int reqWood)
    {
        m_GoldText.text = reqGold.ToString();
        m_WoodText.text = reqWood.ToString();
        UpadtaColorRequirements(reqGold, reqWood);
    }
    
}