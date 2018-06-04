using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovePanel : MonoBehaviour
{
    // 액티브 스크립트
    private Activate m_activate;

    // 월드 오브젝트
    private WorldObject m_worldObject;

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

    // 임시로 발판이 꺼졌다 켜지는 느낌을 하게 할 변수들
    private MeshRenderer m_meshRenderer;

    [Header("임시")]
    [SerializeField]
    private Texture2D m_offEmissionTexture;
    [SerializeField]
    private Texture2D m_onEmissionTexture;

    private void Awake()
    {
        m_activate = GetComponent<Activate>();

        m_worldObject = GetComponent<WorldObject>();

        m_moveGroup = transform.Find("MoveGroup");

        m_meshRenderer = GetComponentInChildren<MeshRenderer>();
        m_meshRenderer.material.SetTexture("_MainTex2", m_offEmissionTexture);
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
                // 임시
                m_meshRenderer.material.SetTexture("_MainTex2", m_onEmissionTexture);

                StartCoroutine(Move());
                break;
            }

            yield return null;
        }
    }

    // 패널 이동
    private IEnumerator Move()
    {
        bool isMoveCompelete = false;

        float addTime = 0f;

        while(true)
        {
            // 이동이 완료되지 않고 시간이 멈춰있지 않다면 실행
            if (!isMoveCompelete && !GameLibrary.Bool_IsGameStop(m_worldObject))
            {

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

                    isMoveCompelete = true;
                }
            }

            // 이동이 완료되고 시간이 멈춰있지 않다면 실행
            if(isMoveCompelete && !GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                addTime += Time.deltaTime;

                if(addTime >= m_waitTime)
                {
                    addTime = 0f;
                    isMoveCompelete = false;
                }
            }

            yield return null;
        }
    }
}
