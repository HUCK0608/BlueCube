using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Multi : WorldObject
{
    private List<MeshRenderer> m_renderers;
    private List<Collider2D> m_collider2D;
    private List<Material> m_defaultMaterials;

    private int m_rendererCount;
    private int m_colliderCount;
    private int m_materialCount;

    private void Awake()
    {
        m_enabled = true;

        m_renderers = new List<MeshRenderer>();
        m_collider2D = new List<Collider2D>();
        m_defaultMaterials = new List<Material>();

        m_renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        m_collider2D.AddRange(GetComponentsInChildren<Collider2D>());

        m_rendererCount = m_renderers.Count;
        m_colliderCount = m_collider2D.Count;

        for(int i = 0; i < m_rendererCount; i++)
            m_defaultMaterials.Add(m_renderers[i].material);

        m_materialCount = m_defaultMaterials.Count;
    }

    public override void RendererEnable(bool value)
    {
        m_enabled = value;

        for (int i = 0; i < m_rendererCount; i++)
            m_renderers[i].enabled = value;
    }

    public override void Collider2DEnable(bool value)
    {
        for (int i = 0; i < m_colliderCount; i++)
            m_collider2D[i].enabled = value;
    }

    public override void ChangeMaterial(E_MaterialType materialType)
    {
        if(materialType.Equals(GameLibrary.Enum_Material_Default))
        {
            for (int i = 0; i < m_materialCount; i++)
                m_renderers[i].material = m_defaultMaterials[i];
        }
        else
        {
            for (int i = 0; i < m_materialCount; i++)
                m_renderers[i].material = GameLibrary.Material_Red;
        }
    }
}
