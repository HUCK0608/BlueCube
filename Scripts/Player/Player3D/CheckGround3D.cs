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

    private float m_distanceToGround;
    /// <summary>땅까지의 거리를 반환</summary>
    public float DistanceToGround { get { return m_distanceToGround; } }

    private void Awake()
    {
        m_checkPointCount = m_checkPoints.Count;
    }

    /// <summary>무언가 충돌하면 true를 반환</summary>
    public bool Check()
    {
        RaycastHit hit;

        // 무시할 레이어 마스크
        int layerMask = (-1) - (GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_BackgroundTrigger);

        float groundCheckDistance = PlayerManager.Instance.Stat.GroundCheckDistance;

        for (int i = 0; i < m_checkPointCount; i++)
        {
            if (GameLibrary.Raycast3D(m_checkPoints[i].position, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                m_distanceToGround = Vector3.Distance(m_checkPoints[i].position, hit.point);

                if (m_distanceToGround <= groundCheckDistance)
                    return true;
            }
        }

        m_distanceToGround = Mathf.Infinity;

        return false;
    }
}
