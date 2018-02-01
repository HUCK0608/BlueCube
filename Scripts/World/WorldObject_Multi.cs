using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Multi : WorldObject
{
    private List<MeshRenderer> m_renderers;

    private int m_rendererCount;

    private void Awake()
    {
        m_renderers = new List<MeshRenderer>();

        m_renderers.AddRange(GetComponentsInChildren<MeshRenderer>());

        m_rendererCount = m_renderers.Count;
    }

    public override void RendererEnable(bool value)
    {
        for (int i = 0; i < m_rendererCount; i++)
            m_renderers[i].enabled = value;
    }
}
