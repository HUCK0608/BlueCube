using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Single : WorldObject
{
    private MeshRenderer m_renderer;
    private Collider2D m_collider2D;
    private Material m_defaultMaterial;
    
    private void Awake()
    {
        m_enabled = true;

        m_renderer = GetComponentInChildren<MeshRenderer>();
        m_collider2D = GetComponentInChildren<Collider2D>();
        m_defaultMaterial = m_renderer.material;
    }

    public override void RendererEnable(bool value)
    {
        m_enabled = value;
        m_renderer.enabled = value;
    }

    public override void Collider2DEnable(bool value)
    {
        if (m_collider2D == null)
            return;

        m_collider2D.enabled = value;
    }

    public override void ChangeMaterial(E_MaterialType materialType)
    {
        if (materialType.Equals(GameLibrary.Enum_Material_Default))
        {
            m_renderer.material = m_defaultMaterial;
        }
        else
        {
            m_renderer.material = GameLibrary.Material_Red;
        }
    }
}
