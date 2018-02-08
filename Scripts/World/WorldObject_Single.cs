using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Single : WorldObject
{
    private MeshRenderer m_renderer;

    private Collider2D m_collider2D;
    
    private void Awake()
    {
        m_enabled = true;

        m_renderer = GetComponentInChildren<MeshRenderer>();

        m_collider2D = GetComponentInChildren<Collider2D>();
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
}
