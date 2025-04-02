using JetBrains.Annotations;
using UnityEngine;

public class ClickToPoint : MonoBehaviour   //管理鼠标ui
{
    private float timer;    //计时器
    private float m_FreTime;        //频率计时器
    private Vector3 m_OriScale;         //原始缩放
    [SerializeField] private float m_Durion = 1f;       //持续时间

    public AnimationCurve m_ScaleCurve;             //缩放曲线
    public SpriteRenderer spriteRenderer;               //精灵渲染器


    private void Start()
    {

        m_OriScale = transform.localScale;
        Debug.Log("初始位置" + m_OriScale);
        
    }

    void Update()       // Update is called once per frame 鼠标实时显示
    {
        timer += Time.deltaTime;
        m_FreTime += Time.deltaTime;

        if(m_FreTime > 1f) m_FreTime = 0;

        float x = m_ScaleCurve.Evaluate(m_FreTime);
        transform.localScale = m_OriScale * x;      //实时缩放

        if(timer > m_Durion*0.9)
        {
            float data = (timer - m_Durion*0.9f) / (m_Durion*0.1f);
            spriteRenderer.color =  new Color(1,1,1,1-data);        //渐变透明度，消失
            Debug.Log("渐变透明度，消失" + data);
        }

        if(timer > m_Durion)
        {
            Destroy(gameObject);
        }
    }
}
