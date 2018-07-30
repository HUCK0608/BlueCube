using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Key : MonoBehaviour
{
    // 콜라이더
    [SerializeField]
    private Collider m_collider;
    [SerializeField]
    private Collider2D m_collider2D;

    ///// <summary>연결된 문</summary>
    //private Door m_connectDoor;
    ///// <summary>연결된 문 설정</summary>
    //public void SetConnectDoor(Door newDoor) { m_connectDoor = newDoor; }

    /// <summary>착지하였는지 여부</summary>
    private bool m_isLanding;
    /// <summary>착지하였을 경우 true를 반환</summary>
    public bool IsLanding { get { return m_isLanding; } }

    /// <summary>착지지점으로 날아가기 시작</summary>
    public void StartFlyToLandingPosition()
    {
        m_collider.enabled = false;
        m_collider2D.enabled = false;
    }

    /// <summary>착지 지점으로 날아가는 로직</summary>
    private IEnumerator FlyToLandingPositionLogic()
    {
        while (true)
        {
            yield return null;
        }
    }
}
