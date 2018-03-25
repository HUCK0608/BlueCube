using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckGround : MonoBehaviour
{
    // 레이
    private Ray m_ray;

    // 체크할 위치들을 인스펙터에서 담을 수 있게 함
    [SerializeField]
    private List<Transform> m_checkPoints;

    // 체크포인터 개수
    private int m_checkPointCount;

    // 레이발사 거리
    [SerializeField]
    private float m_checkDistance;

    private void Awake()
    {
        m_ray = new Ray();
        m_ray.direction = Vector3.down;

        m_checkPointCount = m_checkPoints.Count;
    }

    /// <summary>무언가 충돌하면 true를 반환</summary>
    public bool Check()
    {
        bool isCol = false;

        for(int i = 0; i < m_checkPointCount; i++)
        {
            m_ray.origin = m_checkPoints[i].position;

            if (Physics.Raycast(m_ray, m_checkDistance, GameLibrary.LayerMask_Ignore_BP))
            {
                isCol = true;
                break;
            }
        }

        return isCol;
    }
}
