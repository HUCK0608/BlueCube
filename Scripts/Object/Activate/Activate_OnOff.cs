using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Activate_OnOff : Activate
{
    // 연결 스위치
    [SerializeField]
    private Switch_OnOff m_switch;

    private void Awake()
    {
        StartCoroutine(CheckSwitch());
    }

    // 스위치가 켜졌는지 안켜졌는지 체크하는 코루틴
    private IEnumerator CheckSwitch()
    {
        while(true)
        {
            // 스위치가 켜졌다면 오브젝트 활성화 및 코루틴 종료
            if (m_switch.IsOn)
            {
                m_isActivate = true;
                break;
            }

            yield return null;
        }

    }
}
