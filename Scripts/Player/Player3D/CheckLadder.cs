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

    private Ray m_ray;

    private int m_layerMask;

    private void Awake()
    {
        m_ray = new Ray();

        // 사다리만 통과하는 레이어마스크
        m_layerMask = (1 << 12);
    }

    /// <summary>Direction방향에 사다리를 반환함</summary>
    public Ladder GetLadder(Vector3 directon)
    {
        Ladder ladder = null;

        m_ray.origin = m_forwardPoint.position;
        m_ray.direction = directon;

        RaycastHit hit;
        if(Physics.Raycast(m_ray, out hit, m_forwardCheckDistance, m_layerMask))
        {
            ladder = hit.transform.GetComponentInParent<Ladder>();
        }

        return ladder;
    }

    /// <summary>Direction방향에 사다리가 있으면 true를 반환</summary>
    public bool IsOnLadder(Vector3 direction)
    {
        bool isLadder = false;

        m_ray.origin = m_forwardPoint.position;
        m_ray.direction = direction;

        if (Physics.Raycast(m_ray, m_forwardCheckDistance, m_layerMask))
        {
            isLadder = true;
        }

        return isLadder;
    }

    /// <summary>사다리의 아래부분이면 true를 반환</summary>
    public bool IsLadderDown()
    {
        bool isLadderDown = false;

        m_ray.origin = m_downPoint.position;
        m_ray.direction = Vector3.down;

        // 무시할 레이어 마스크
        int layerMask = (-1) - (GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_BackgroundTrigger);

        if (Physics.Raycast(m_ray, m_downCheckDistance, layerMask))
        {
            isLadderDown = true;
        }

        return isLadderDown;
    }
}
