using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Single : WorldObject
{
    private MeshRenderer m_renderer;
    private Collider2D m_collider2D;
    private Material m_defaultMaterial;

    protected override void Awake()
    {
        base.Awake();

        m_renderer = GetComponentInChildren<MeshRenderer>();
        m_collider2D = GetComponentInChildren<Collider2D>();
        m_defaultMaterial = m_renderer.material;
    }

    public override void SetRendererEnable(bool value)
    {
        base.SetRendererEnable(value);

        m_renderer.enabled = value;
    }

    public override void SetCollider2DEnable(bool value)
    {
        if (m_collider2D == null)
            return;

        m_collider2D.enabled = value;
    }

    public override void SetMaterial(E_MaterialType materialType)
    {
        if (materialType.Equals(E_MaterialType.Default))
        {
            m_renderer.material = m_defaultMaterial;
        }
        else
        {
            m_renderer.material = GameLibrary.Material_CanChange;
        }
    }
}
