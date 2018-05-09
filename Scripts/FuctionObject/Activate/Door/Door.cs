using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_OpenDir { Left, Right }

public sealed class Door : MonoBehaviour
{
    // 액티브 스크립트
    private Activate m_activate;

    // 월드 오브젝트
    private WorldObject m_worldObject;

    // 열리는 위치
    [SerializeField]
    private E_OpenDir m_openDir;

    // 문이 열리는 속도
    [SerializeField]
    private float m_moveSpeed;

    // 문 열리기전 딜레이
    [SerializeField]
    private float m_delayTime;

    // 이동할 위치
    private Vector3 m_openPos;

    // 모델의 x 크기
    private float m_width;

    private void Awake()
    {
        m_activate = GetComponentInParent<Activate>();

        m_worldObject = GetComponentInParent<WorldObject>();

        InitDoor();

        StartCoroutine(CheckActivate());
    }

    // 문 초기화
    private void InitDoor()
    {
        m_width = GetComponentInChildren<BoxCollider>().bounds.extents.x * 2f;

        if (m_openDir == E_OpenDir.Left)
            m_openPos = new Vector3(transform.position.x - m_width, transform.position.y, transform.position.z);
        else if (m_openDir == E_OpenDir.Right)
            m_openPos = new Vector3(transform.position.x + m_width, transform.position.y, transform.position.z);
    }

    // 활성화 체크
    private IEnumerator CheckActivate()
    {
        while(true)
        {
            if (m_activate.IsActivate)
                break;

            yield return null;
        }

        StartCoroutine(Open());
    }

    // 문 열기
    private IEnumerator Open()
    {
        yield return new WaitForSeconds(m_delayTime);

        while(true)
        {
            // 시점변환중이 아니고 탐지모드가 아니고 2D가 아닐경우 실행
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                transform.position = Vector3.MoveTowards(transform.position, m_openPos, m_moveSpeed * Time.deltaTime);

                if (transform.position == m_openPos)
                    break;
            }
            yield return null;
        }
    }
}
