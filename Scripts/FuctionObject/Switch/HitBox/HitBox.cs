using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HitBox : MonoBehaviour
{
    private Switch m_switch;

    // 부셔질 목록
    [SerializeField]
    private List<GameObject> m_breakList;

    private void Awake()
    {
        m_switch = GetComponent<Switch>();
        StartCoroutine(CheckBroken());
    }

    // 스위치가 부셔졌는지 체크
    private IEnumerator CheckBroken()
    {
        while(true)
        {
            if (m_switch.IsOn)
            {
                for (int i = 0; i < m_breakList.Count; i++)
                    m_breakList[i].SetActive(false);

                break;
            }

            yield return null;
        }
    }
}
