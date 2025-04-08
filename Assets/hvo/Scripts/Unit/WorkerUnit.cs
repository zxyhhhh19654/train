
using UnityEngine;

public class WorkerUnit : HumanoidUnit
{
    protected override void UpdateBehaviour()
    {
        CheckForCloseObject();
    }

    private void CheckForCloseObject()
    {
        
        var hits = RunProximityObjctDetection();

        foreach (var hit in hits)
        {
            if(hit.gameObject == this.gameObject) continue; // Ignore self
            Debug.Log(hit.gameObject.name);
        }
    }
}