using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_On : Switch
{
    protected MeshFilter m_meshFilter;

    [SerializeField]
    protected Mesh m_onMesh;

    protected virtual void Awake()
    {
        m_meshFilter = GetComponentInChildren<MeshFilter>();
    }

    public virtual void SwitchOn()
    {
        m_isOn = true;
        m_meshFilter.mesh = m_onMesh;
    }
}
