using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckGround2D : MonoBehaviour
{
    // 체크할 위치들을 인스펙터에서 담을 수 있게 함
    [SerializeField]
    private List<Transform> m_checkPoints;

    // 체크포인터 개수
    private int m_checkPointCount;

    // 레이발사 거리
    [SerializeField]
    private float m_checkDistance;

    // 땅이랑 띄워줄 거리
    [SerializeField]
    private float m_onGroundUpPosition;

    private float m_onGroundPositionY;
    /// <summary>땅 위 y좌표를 반환함</summary>
    public float OnGroundPositionY { get { return m_onGroundPositionY; } }

    private void Awake()
    {
        m_checkPointCount = m_checkPoints.Count;
    }

    /// <summary>무언가 충돌하면 true를 반환</summary>
    public bool Check()
    {
        bool isCol = false;

        RaycastHit2D hit;

        // 무시할 레이어 마스크
        int layerMask = (-1) - (GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_BackgroundTrigger);

        for (int i = 0; i < m_checkPointCount; i++)
        {
            if (GameLibrary.Raycast2D(m_checkPoints[i].position, Vector2.down, out hit, m_checkDistance, layerMask))
            {
                isCol = true;
                m_onGroundPositionY = hit.point.y + m_onGroundUpPosition;
                break;
            }
        }

        return isCol;
    }
}
