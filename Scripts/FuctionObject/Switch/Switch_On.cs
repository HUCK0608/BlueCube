using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch_On : Switch
{
    [Header("[Switch On]")]
    [Space(-5f)]
    [Header("- Don't Touch")]
    /// <summary>바꿀 메쉬</summary>
    [SerializeField]
    protected MeshFilter m_changeMesh;

    /// <summary>스위치가 켜졌을 때 메쉬</summary>
    [SerializeField]
    protected Mesh m_onMesh;

    /// <summary>스위치가 켜지는 로직이 실행중일경우 true를 반환</summary>
    protected bool m_isOnLogic;

    /// <summary>스위치를 킨다</summary>
    public void SwitchOn()
    {
        // 이미 스위치가 켜져있거나 로직이 실행중일 경우 리턴
        if (m_isOn || m_isOnLogic)
            return;

        StartCoroutine(SwitchOnLogic());
    }

    /// <summary>스위치 로직(반드시 자식에서 구현)</summary>
    protected abstract IEnumerator SwitchOnLogic();

    /// <summary>현재 메쉬를 켜졌을 때 메쉬로 바꿈</summary>
    protected void ChangeOnMesh()
    {
        m_changeMesh.mesh = m_onMesh;
    }
}
