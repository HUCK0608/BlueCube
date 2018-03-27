using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckGround3D : MonoBehaviour
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

        RaycastHit hit;

        for(int i = 0; i < m_checkPointCount; i++)
        {
            if (GameLibrary.Raycast3D(m_checkPoints[i].position, Vector3.down, out hit, m_checkDistance, GameLibrary.LayerMask_Ignore_BP))
            {
                isCol = true;
                m_onGroundPositionY = hit.point.y + m_onGroundUpPosition;
                break;
            }
        }

        return isCol;
    }
}
