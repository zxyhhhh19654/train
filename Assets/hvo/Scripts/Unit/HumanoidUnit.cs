using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class HumanoidUnit : Unit
{
    Vector2 lastPosition;   //上一个位置
    Vector2 velocity;   //速度

    //float velocity;

    void Start()
    {
        lastPosition = transform.position;
    }
    void Update()
    {
        
        velocity = new Vector2(transform.position.x - lastPosition.x, transform.position.y - lastPosition.y) / Time.deltaTime;
        lastPosition = transform.position;
        if (velocity.magnitude <= 0) IsMoving = false;
        else IsMoving = true;
        m_Animator.SetFloat("Speed",velocity.magnitude);
    }
}
