using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_OnOff : Switch
{
    private MeshFilter m_meshFilter;

    [SerializeField]
    protected Mesh m_onMesh, m_offMesh;

    protected virtual void Awake()
    {
        m_meshFilter = GetComponentInChildren<MeshFilter>();
        m_meshFilter.mesh = m_offMesh;
    }

    public virtual void SwitchOn()
    {
        m_isOn = true;
        m_meshFilter.mesh = m_onMesh;
    }

    public virtual void SwitchOff()
    {
        m_isOn = false;
        m_meshFilter.mesh = m_offMesh;
    }
}
