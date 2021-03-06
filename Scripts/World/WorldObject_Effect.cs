﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject_Effect : WorldObject
{
    private List<ParticleSystem> m_particles;
    private Collider2D m_collider2D;

    private int m_particleCount;

    private bool m_includeChildren;

    private void Awake()
    {
        m_enabled = true;

        m_particles = new List<ParticleSystem>();

        m_particles.AddRange(GetComponentsInChildren<ParticleSystem>());

        m_collider2D = GetComponentInChildren<Collider2D>();

        m_particleCount = m_particles.Count;

        m_includeChildren = true;
    }

    public override void RendererEnable(bool value)
    {
        m_enabled = value;

        if (value)
        {
            for (int i = 0; i < m_particleCount; i++)
                m_particles[i].Play(m_includeChildren);
        }
        else
        {
            for (int i = 0; i < m_particleCount; i++)
            {
                m_particles[i].Stop(m_includeChildren);
                m_particles[i].Clear(m_includeChildren);
            }
        }
    }

    public override void Collider2DEnable(bool value)
    {
        if (m_collider2D != null)
            m_collider2D.enabled = value;
    }

}
