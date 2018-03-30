using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovePanel : MonoBehaviour
{
    // 액티브 스크립트
    private Activate m_activate;

    // 경로 스크립트
    [SerializeField]
    private MovePath m_path;

    // 현재 경로 값
    private int m_currentPath;

    // 이동 그룹
    private Transform m_moveGroup;

    // 이동속도
    [SerializeField]
    private float m_moveSpeed;

    // 다음 경로에 도착하면 대기시간
    [SerializeField]
    private float m_waitTime;

    private void Awake()
    {
        m_activate = GetComponent<Activate>();

        m_moveGroup = transform.Find("MoveGroup");

        StartCoroutine(CheckActivate());
    }

    // 활성화 체크 코루틴
    private IEnumerator CheckActivate()
    {
        while(true)
        {
            // 활성화가 되었다면 패널 이동 코루틴을 시작하고 현재 코루틴 종료
            if(m_activate.IsActivate)
            {
                StartCoroutine(Move());
                break;
            }

            yield return null;
        }
    }

    // 패널 이동
    private IEnumerator Move()
    {
        while(true)
        {
            // 시점변환중이 아니고 탐지모드가 아니고 2D가 아닐경우 실행
            if (!GameLibrary.Bool_IsGameStop_Old)
            {
                yield return new WaitForSeconds(m_waitTime);

                m_moveGroup.position = Vector3.MoveTowards(m_moveGroup.position, m_path.PathPosition(m_currentPath), m_moveSpeed * Time.deltaTime);

                // 정해진 경로에 도착했다면 다음 경로로 바꿔줌
                if (m_moveGroup.position == m_path.PathPosition(m_currentPath))
                {
                    // 현재 경로가 마지막 경로라면 처음 경로로 바꿈
                    if (m_currentPath.Equals(m_path.PathCount - 1))
                        m_currentPath = 0;
                    // 아닐 경우 다음 경로로 바꿈
                    else
                        m_currentPath++;
                }
            }
            yield return null;
        }
    }
}
