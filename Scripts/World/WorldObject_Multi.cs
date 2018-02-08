using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Multi : WorldObject
{
    private List<MeshRenderer> m_renderers;
    private List<Collider2D> m_collider2D;

    private int m_rendererCount;
    private int m_colliderCount;

    private void Awake()
    {
        m_enabled = true;

        m_renderers = new List<MeshRenderer>();
        m_collider2D = new List<Collider2D>();

        m_renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        m_collider2D.AddRange(GetComponentsInChildren<Collider2D>());

        m_rendererCount = m_renderers.Count;
        m_colliderCount = m_collider2D.Count;
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
}
