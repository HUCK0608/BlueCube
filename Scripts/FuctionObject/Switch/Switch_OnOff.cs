using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch_OnOff : Switch_On
{
    [Header("[Switch Off Settings]")]
    [Space(-5f)]
    [Header("- Don't Touch")]

    /// <summary>스위치가 꺼졌을 때 메쉬</summary>
    [SerializeField]
    protected Mesh m_offMesh;

    /// <summary>스위치가 꺼지는 로직이 실행중일 경우 true를 반환</summary>
    protected bool m_isOffLogic;

    /// <summary>스위치를 끈다</summary>
    public virtual void SwitchOff()
    {
        // 이미 스위치가 꺼져있거나 로직이 실행중일 경우 리턴
        if (!m_isOn || m_isOffLogic)
            return;

        StartCoroutine(SwitchOffLogic());
    }

    /// <summary>스위치가 꺼지는 로직(반드시 자식에서 구현)</summary>
    protected abstract IEnumerator SwitchOffLogic();

    /// <summary>스위치가 꺼졌을 때 꺼진 메쉬로 변경</summary>
    protected void ChangeOffMesh()
    {
        m_changeMesh.mesh = m_offMesh;
    }
}
