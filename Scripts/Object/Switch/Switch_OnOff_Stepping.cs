using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_OnOff_Stepping : Switch_OnOff
{
    // 스위치가 꺼지는 딜레이
    [SerializeField]
    private float m_offDelay;

    // 스위치가 꺼지는 타이머 코루틴
    private Coroutine m_offTimerCoroutine;

    // 스위치가 꺼지는 타이머가 활성화 되어있는지
    private bool m_isOffTimerActive;

    // 메쉬
    private MeshFilter m_meshFilter;

    protected override void Awake()
    {
        base.Awake();
        InitMesh();
    }

    private void InitMesh()
    {
        m_meshFilter = GetComponentInChildren<MeshFilter>();
    }

    /// <summary>스위치 켜기</summary>
    public override void SwitchOn()
    {
        if (m_isOffTimerActive)
            SwitchOffTimerOff();

        m_isOn = true;

        // 켜진 메쉬로 바꿈
        m_meshFilter.mesh = m_onMesh;
    }

    /// <summary>스위치 끄기</summary>
    public override void SwitchOff()
    {
        // 타이머가 활성화 되어있지 않은 경우에만 실행
        if (!m_isOffTimerActive)
            SwitchOffTimerOn();
    }

    // 스위치가 꺼지는 타이머 켜기
    private void SwitchOffTimerOn()
    {
        m_offTimerCoroutine = StartCoroutine(SwitchOffOrder());
    }

    // 스위치가 꺼지는 타이머 끄기
    private void SwitchOffTimerOff()
    {
        StopCoroutine(m_offTimerCoroutine);
        m_isOffTimerActive = false;
    }

    // 스위치가 꺼지는 순서
    private IEnumerator SwitchOffOrder()
    {
        m_isOffTimerActive = true;
        // 타이머 대기
        yield return StartCoroutine(GameLibrary.Timer(m_offDelay));
        // 꺼진 메쉬로 바꿈
        m_meshFilter.mesh = m_offMesh;
        m_isOffTimerActive = false;
        m_isOn = false;
    }
}
