using UnityEngine;

public class Unit : MonoBehaviour
{
[Header("角色属性选择")]
   public bool IsMoving;//角色是否在移动
   public bool IsSelected;//角色是否被选中
   
   [Header("角色关联组件")]
   [SerializeField] protected Animator m_Animator;
   [SerializeField] private AIPawn m_AIPawn;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private ActionSo[] m_Actions;


    private Material heightMaterial;//点击特效
    private Material oriMaterial;   //原本的材质
  
    
    public bool IsMoving1 => IsMoving;  //获取角色是否在移动
    public ActionSo[] Actions => m_Actions;  //获取角色的技能
    void Awake()
    {
        if(TryGetComponent<AIPawn>(out AIPawn aIPawn))
        {
            m_AIPawn = aIPawn;
        }
        if(TryGetComponent<Animator>(out  Animator animator))
        {
            m_Animator = animator;
        }
        if(TryGetComponent<SpriteRenderer>(out  SpriteRenderer m_SpriteRenderer1))
        {
            m_SpriteRenderer = m_SpriteRenderer1;
        }
        oriMaterial = m_SpriteRenderer.material;
        heightMaterial = Resources.Load<Material>("Materials/Outline");

    }

    public void MoveTo(Vector2 destPosition)
   {
        float x = destPosition.x - transform.position.x;
        m_SpriteRenderer.flipX = x < 0;
        m_AIPawn.SetDestPosition(destPosition);
        //Debug.Log("MoveTo");
   }


    //点击特效------------------------------------------------------------------------------
   public void Select()//角色被选择时的特效
    {
        SelectHeight();
    }
    public void UnSelect()//角色没有被选择时的特效
    {
        UnSelectHeight();
    }
    private void SelectHeight()//点击特效显现
    {
        m_SpriteRenderer.material = heightMaterial;
    }
    private void UnSelectHeight()//取消点击特效显现
    {
        m_SpriteRenderer.material = oriMaterial;
    }

}
