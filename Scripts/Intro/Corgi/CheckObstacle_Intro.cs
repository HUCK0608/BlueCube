using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckObstacle_Intro : MonoBehaviour
{
    [SerializeField]
    private Transform m_groundCheckPoint;

    [SerializeField]
    private List<Transform> m_forwardCheckPoints;
    private int m_checkPointCount;

    private void Awake()
    {
        m_checkPointCount = m_forwardCheckPoints.Count;
    }

    /// <summary>장애물이 있을 경우 true를 반환</summary>
    public bool Check()
    {
        float checkDistance = CorgiController_Intro.Instance.Stat.ObstacleCheckDistance;

        int layerMask = (-1) - (GameLibrary.LayerMask_PanIntro.Shift() |
                                GameLibrary.LayerMask_CorgiIntro.Shift());

        RaycastHit2D hit;
        // 전방이 땅이 아니라면 장애물이 있다고 알림
        if (!GameLibrary.Raycast2D(m_groundCheckPoint.position, m_groundCheckPoint.forward, checkDistance, layerMask))
            return true;

        // 정면에 장애물이 있으면 장애물이 있다고 알림
        for (int i = 0; i < m_checkPointCount; i++)
            if (GameLibrary.Raycast2D(m_forwardCheckPoints[i].position, m_forwardCheckPoints[i].forward, out hit, checkDistance, layerMask))
                return true;

        return false;
    }
}
