using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Door_Activate : Door
{
    /// <summary>활성화</summary>
    private Activate m_activate;

    private int m_connectSwitchCount;

    protected override void Start()
    {
        base.Start();

        m_activate = GetComponent<Activate>();

        StartCoroutine(CheckOnActivate());
    }

    /// <summary>활성화 되었는지 체크</summary>
    private IEnumerator CheckOnActivate()
    {
        while (true)
        {
            if (m_activate.IsActivate)
                break;

            yield return null;
        }

        // 활성화 되어있을 경우 문 열기 실행
        OpenDoor();
    }
}
