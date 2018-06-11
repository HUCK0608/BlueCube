using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckLadder : MonoBehaviour
{
    // 정면 체크할 위치
    [SerializeField]
    private Transform m_forwardPoint;

    // 아래쪽 체크할 위치
    [SerializeField]
    private Transform m_downPoint;

    // 정면 체크 길이
    [SerializeField]
    private float m_forwardCheckDistance;

    // 아래쪽 체크 길이
    [SerializeField]
    private float m_downCheckDistance;

    /// <summary>사다리 모음</summary>
    private Dictionary<Transform, Ladder> m_ladders;

    private Ladder m_latelyLadder;
    /// <summary>제일 최근 사다리</summary>
    public Ladder LatelyLadder { get { return m_latelyLadder; } }

    private void Awake()
    {
        m_ladders = new Dictionary<Transform, Ladder>();
    }

    /// <summary>Direction방향에 사다리가 있으면 true를 반환</summary>
    public bool IsOnLadder(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(m_forwardPoint.position, direction, out hit, m_forwardCheckDistance, GameLibrary.LayerMask_Ladder))
        {
            if (!m_ladders.ContainsKey(hit.transform))
                m_ladders.Add(hit.transform, hit.transform.GetComponentInParent<Ladder>());

            m_latelyLadder = m_ladders[hit.transform];

            return true;
        }

        return false;
    }

    /// <summary>사다리의 아래부분이면 true를 반환</summary>
    public bool IsLadderDown()
    {
        // 무시할 레이어 마스크
        int layerMask = (-1) - (GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_BackgroundTrigger);

        if (Physics.Raycast(m_downPoint.position, Vector3.down, m_downCheckDistance, layerMask))
            return true;

        return false;
    }
}
