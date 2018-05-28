using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Door : MonoBehaviour
{
    [Header("Don't Change")]

    /// <summary>문</summary>
    [SerializeField]
    private Transform m_door;

    /// <summary>문의 도착지</summary>
    [SerializeField]
    private Vector3 m_destination;

    /// <summary>콜라이더 2D (Vertical에서만 사용)</summary>
    [SerializeField]
    private GameObject m_collider2D;

    [Header("Can Change")]

    /// <summary>문이 열리는 속도</summary>
    [SerializeField]
    private float m_moveSpeed;

    /// <summary>월드 오브젝트</summary>
    private WorldObject m_worldObject;
    /// <summary>활성화 스크립트</summary>
    private Activate m_activate;

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_activate = GetComponent<Activate>();

        StartCoroutine(CheckActivate());
    }

    /// <summary>활성화 되었는지 체크</summary>
    private IEnumerator CheckActivate()
    {
        yield return new WaitUntil(() => m_activate.IsActivate);

        StartCoroutine(DoorOpenLogic());
    }

    /// <summary>문이 열리는 로직</summary>
    private IEnumerator DoorOpenLogic()
    {
        while(true)
        {
            if(!GameLibrary.Bool_IsGameStop(m_worldObject))
                m_door.localPosition = Vector3.MoveTowards(m_door.localPosition, m_destination, m_moveSpeed * Time.deltaTime);

            if (m_door.localPosition.Equals(m_destination))
                break;

            yield return null;
        }

        if (m_collider2D != null)
            m_collider2D.SetActive(false);

        m_door.gameObject.SetActive(false);
    }
}
