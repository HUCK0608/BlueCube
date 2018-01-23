using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HitBox : MonoBehaviour
{
    // 부셔질 목록
    [SerializeField]
    private List<GameObject> m_breakList;

    private Switch_Hit m_switch;

    private void Awake()
    {
        m_switch = GetComponent<Switch_Hit>();

        StartCoroutine(CheckBroken());
    }

    // 부셔졌는지 체크하는 코루틴
    private IEnumerator CheckBroken()
    {
        while(true)
        {
            if (m_switch.IsBroken)
            {
                for (int i = 0; i < m_breakList.Count; i++)
                    m_breakList[i].SetActive(false);

                break;
            }

            yield return null;
        }
    }
}
