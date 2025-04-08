

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
        var structureGo = new GameObject(m_BuildAcition.name);
        var renderer = structureGo.AddComponent<SpriteRenderer>();

        renderer.sortingOrder = 999;
        renderer.sprite = m_BuildAcition.FoundationSprite;
        structureGo.transform.position = placementPosition;
    }
    
}