using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject_Effect : WorldObject
{
    private List<ParticleSystem> m_particles;

    private int m_particleCount;

    private bool m_includeChildren;

    private void Awake()
    {
        m_particles = new List<ParticleSystem>();

        m_particles.AddRange(GetComponentsInChildren<ParticleSystem>());

        m_particleCount = m_particles.Count;

        m_includeChildren = true;
    }

    public override void RendererEnable(bool value)
    {
        if (value)
        {
            for (int i = 0; i < m_particleCount; i++)
                m_particles[i].Play(m_includeChildren);
        }
        else
        {
            for (int i = 0; i < m_particleCount; i++)
                m_particles[i].Stop(m_includeChildren);
        }
    }

}
