using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_OnOff_PowerLine : Switch_OnOff
{
    /// <summary>라인</summary>
    private Special_PowerLine_Line m_line;

    private WaitUntil m_switchOnLogicWaitUntil;
    private WaitUntil m_switchOffLogicWaitUntil;

    private void Start()
    {
        m_line = GetComponent<Special_PowerLine_Line>();

        m_switchOnLogicWaitUntil = new WaitUntil(() => m_line.IsConnectedPower);
        m_switchOffLogicWaitUntil = new WaitUntil(() => !m_line.IsConnectedPower);

        StartCoroutine(SwitchOnLogic());
    }

    /// <summary>스위치가 켜지는 로직</summary>
    protected override IEnumerator SwitchOnLogic()
    {
        yield return m_switchOnLogicWaitUntil;

        m_isOn = true;

        StartCoroutine(SwitchOffLogic());
    }

    /// <summary>스위치가 꺼지는 로직</summary>
    protected override IEnumerator SwitchOffLogic()
    {
        yield return m_switchOffLogicWaitUntil;

        m_isOn = false;

        StartCoroutine(SwitchOnLogic());
    }
}
