
using UnityEngine;


public class AIPawn : MonoBehaviour     //AI角色类，继承自MonoBehaviour类
{
    private Vector3 ? destPosition;

    private float Speed = 4f;

    // void Start()
    // {
    //     destPosition = new Vector3(1,1,1);
    // }

    private void Update()       // Update is called once per frame
    {
        if(destPosition.HasValue)
        {
            var  dir = destPosition.Value - transform.position;
            transform.position += dir.normalized * Speed * Time.deltaTime;

        //if()

            if(Vector3.Distance((Vector3)destPosition, transform.position) < .1f)
            {
                destPosition = null;
            }
        }
        
    }
    public void SetDestPosition(Vector2 destitionPosition)
    {
        destPosition = destitionPosition;
       //Debug.Log("SetDestPosition");
    }
    
}
