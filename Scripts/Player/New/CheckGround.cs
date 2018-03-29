using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckGround : MonoBehaviour
{
    private Ray m_ray;

    [SerializeField]
    private List<Transform> m_checkPoints;
    [SerializeField]
    private float m_checkDistance;
    private int m_checkCount;

    private void Awake()
    {
        m_ray = new Ray();
        m_ray.direction = Vector3.down;

        m_checkCount = m_checkPoints.Count;
    }

    /// <summary>무언가 충돌하면 true를 반환</summary>
    public bool Check()
    {
        bool isCol = false;

        for(int i = 0; i < m_checkCount; i++)
        {
            m_ray.origin = m_checkPoints[i].position;
            if (Physics.Raycast(m_ray, m_checkDistance, GameLibrary.LayerMask_Ignore_BPE))
            {
                isCol = true;
                break;
            }
        }

        return isCol;
    }
}
