using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    protected WorldObject m_worldObject;

    /// <summary>버튼이 내려갔을 때 위치</summary>
    [SerializeField]
    protected Vector3 m_buttonOnPosition;

    /// <summary>버튼 이동속도</summary>
    [SerializeField]
    protected float m_buttonMoveSpeed;

    /// <summary>버튼</summary>
    [SerializeField]
    protected Transform m_button;
    /// <summary>버튼 메쉬필터</summary>
    protected MeshFilter m_buttonMeshFilter;

    /// <summary>버튼이 켜졌을 때 메쉬</summary>
    protected static Mesh m_buttonOnMesh;

    /// <summary>스위치가 켜져있는지 여부</summary>
    protected bool m_isOn;
    public bool IsOn { get { return m_isOn; } }

    /// <summary>현재 버튼이 이동중이면 true를 반환</summary>
    protected bool m_isButtonMove;
    
    protected virtual void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();

        m_buttonMeshFilter = m_button.GetComponent<MeshFilter>();

        if (m_buttonOnMesh == null)
            m_buttonOnMesh = (Resources.Load("Model/FunctionObject/Switch/Switch_SteppingOnButton") as GameObject).GetComponentInChildren<MeshFilter>().sharedMesh;
    }
}
