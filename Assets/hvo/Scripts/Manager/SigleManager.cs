using UnityEngine;

public abstract class SigleManager<T> : MonoBehaviour where T:MonoBehaviour
{
    protected virtual void Awake()
    {
        T[] managers = FindObjectsByType<T>(FindObjectsSortMode.None);
        if(managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    public static T Get()
    {
        var tag = typeof(T).Name;
        GameObject managerobject = GameObject.FindWithTag(tag);
        if(managerobject != null)
        {
            return managerobject.GetComponent<T>();
        }
        
        GameObject go = new(tag);
        go.tag = tag;
        return go.AddComponent<T>();   
    }
    

}
