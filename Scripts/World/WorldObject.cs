using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    protected bool m_enabled;
    public bool Enabled { get { return m_enabled; } }

    public virtual void RendererEnable(bool value)
    {

    }

    public virtual void Collider2DEnable(bool value)
    {

    }

    public virtual void ChangeMaterial(E_MaterialType materialType)
    {

    }
}
