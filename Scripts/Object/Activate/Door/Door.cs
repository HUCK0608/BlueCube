using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_OpenDir { Left, Right }

public sealed class Door : MonoBehaviour
{
    // 액티브 스크립트
    private Activate m_activate;

    // 열리는 위치
    [SerializeField]
    private E_OpenDir m_openDir;

    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private float m_delayTime;

    // 이동할 위치
    private Vector3 m_openPos;

    // 모델의 x 크기
    private float m_width;

    // 열리고나서 부셔져야 하는 콜라이더
    private Collider2D m_collider2D;

    private WorldObject m_worldObject;

    private void Awake()
    {
        m_activate = GetComponent<Activate>();

        m_collider2D = GetComponentInChildren<Collider2D>();

        m_worldObject = GetComponent<WorldObject>();

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
            // 시점변환중이 아니고 활성화 상태일경우에만 이동
            if (!GameManager.Instance.PlayerManager.Skill_CV.IsChanging && m_worldObject.Enabled)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_openPos, m_moveSpeed * Time.deltaTime);

                if (transform.position == m_openPos)
                    break;
            }
            yield return null;
        }

        m_collider2D.enabled = false;
    }
}
