using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Activate_Switch : Activate
{
    // 스위치들
    [SerializeField]
    private List<Switch> m_switches;

    // 스위치 개수
    private int m_switchCount;

    private void Awake()
    {
        m_switchCount = m_switches.Count;

        StartCoroutine(CheckSwitches());
    }

    // 스위치들이 켜져있는지 체크
    private IEnumerator CheckSwitches()
    {
        while (true)
        {
            int onSwitchCount = 0;

            for (int i = 0; i < m_switchCount; i++)
            {
                if (m_switches[i].IsOn)
                    onSwitchCount++;
            }

            // 스위치들이 모두 켜졌다면 활성화
            if (onSwitchCount.Equals(m_switchCount))
                break;

            yield return null;
        }

        m_isActivate = true;
    }
}
