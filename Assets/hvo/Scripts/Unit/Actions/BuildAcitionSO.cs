

using UnityEngine;
using UnityEngine.UI;

//这是一个实现build的Execute实现（利用StartBuildProcess跳转至manager，再由manager跳转至PlacementProcessment）
[CreateAssetMenu(fileName = "BuildAction", menuName = "hvo/Actions/BuildAction")]
public class BuildAcitionSO : ActionSo
{
    [SerializeField]private Sprite m_PlacementSprite;   //放置图片
    [SerializeField]private Image m_FoundationSprite;   //基础图标（正在建造）
    [SerializeField]private Image m_CompletSprite;      //完成图片
    public Sprite PlacementSprite => m_PlacementSprite;     
    public Image FoundationSprite => m_FoundationSprite;  
    public Image CompletSprite => m_CompletSprite;

    [SerializeField] private Vector3Int m_BuildingSize;     //建筑瓦片大小
    [SerializeField] private Vector3Int m_OriginOffset;         //建筑原点偏移量

    public Vector3Int BuildingSize => m_BuildingSize;
    public Vector3Int OriginOffset => m_OriginOffset;

    [SerializeField] private int m_goldCost; //建筑预制体
    public int GoldCost => m_goldCost; //建筑预制体的金币消耗
    [SerializeField] private int m_woodCost; //建筑预制体 
    public int WoodCost => m_woodCost; //建筑预制体的食物消耗

    public override void Execute(GameManager manager)
    {
       manager.StartBuildProcess(this);
    }
}