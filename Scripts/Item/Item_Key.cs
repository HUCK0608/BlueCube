using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Key : MonoBehaviour
{
    // 콜라이더
    private Collider m_collider;
    private Collider2D m_collider2D;

    /// <summary>연결된 문</summary>
    private Door_Key m_connectDoor;
    /// <summary>연결된 문 설정</summary>
    public void SetConnectDoor(Door_Key newDoor) { m_connectDoor = newDoor; }

    /// <summary>착지하였는지 여부</summary>
    private bool m_isLanding;
    /// <summary>착지하였을 경우 true를 반환</summary>
    public bool IsLanding { get { return m_isLanding; } }

    /// <summary>이동속도</summary>
    [SerializeField]
    private float m_moveSpeed = 10f;

    private void Awake()
    {
        m_collider = GetComponentInChildren<Collider>();
        m_collider2D = GetComponentInChildren<Collider2D>();
    }

    /// <summary>착지지점으로 날아가기 시작</summary>
    public void StartFlyToLandingPosition()
    {
        m_collider.enabled = false;
        m_collider2D.enabled = false;

        StartCoroutine(FlyToLandingPositionLogic());
    }

    /// <summary>착지 지점으로 날아가는 로직</summary>
    private IEnumerator FlyToLandingPositionLogic()
    {
        Vector3 landingPosition = m_connectDoor.GetKeyLandingPosition();

        // 안착 지점으로 날라가는 로직
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, landingPosition, m_moveSpeed * Time.deltaTime);

            if (transform.position.Equals(landingPosition))
                break;

            yield return null;
        }

        m_connectDoor.CompleteLanding();
        gameObject.SetActive(false);
    }
}
