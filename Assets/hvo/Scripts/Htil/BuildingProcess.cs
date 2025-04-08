

using UnityEngine;

public class BuildingProcess
{
    private BuildAcitionSO m_BuildAcition;

    public BuildingProcess(
        BuildAcitionSO buildAcition,
        Vector3 placementPosition
    )
    {
        m_BuildAcition = buildAcition;
        var structure = Object.Instantiate(m_BuildAcition.StructureUnifab);
        structure.Renderer.sprite = m_BuildAcition.PlacementSprite;
        structure.transform.position = placementPosition;
        structure.RegisterProcess(this);
    }


    public void Update()
    {
        Debug.Log("BuildingProcess Update called.");
        // Update the building process here if needed
    }
    
}