using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Activate_Hit : Activate
{
    // 연결 스위치
    [SerializeField]
    private List<Switch_Hit> m_switch;

    // 연결된 스위치 개수
    private int m_switchAmount;

    private void Awake()
    {
        m_switchAmount = m_switch.Count;

        StartCoroutine(CheckSwitch());
    }

    // 스위치가 다 켜졌는지 체크하는 코루틴
    private IEnumerator CheckSwitch()
    {
        // 활성화가 안되었을 경우만 루트
        while(!m_isActivate)
        {
            // 부셔진 스위치 개수
            int brokeSwitchAmount = 0;

            // 모든 스위치 체크
            for (int i = 0; i < m_switchAmount; i++)
            {
                // i번째 스위치가 부셔졌다면
                if (m_switch[i].IsBroken)
                {
                    // 부셔진 스위치 개수 증가
                    brokeSwitchAmount++;

                    // 부셔진 스위치 개수가 연결된 스위치 개수랑 같다면 오브젝트 활성화 및 코루틴 종료
                    if (brokeSwitchAmount == m_switchAmount)
                    {
                        m_isActivate = true;
                        break;
                    }
                }
            }

            yield return null;
        }
    }
}
