using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ColliderCheck2D : MonoBehaviour
{
    private Collider2D m_collider2D;

    private Ray m_ray;
    private int m_layerMask;

    private void Awake()
    {
        m_collider2D = GetComponent<Collider2D>();

        // 총알, 플레이어 레이어 제외
        m_layerMask = (-1) - ((1 << 8) | (1 << 11));
    }

    // 콜라이더 체크
    public void ColliderCheck()
    {
        m_ray = new Ray(transform.position, Vector3.back);

        // 2D 카메라 방향으로 레이를 쏴서 무언가 맞으면 트리거를 킴
        if(Physics.Raycast(m_ray, 1000f, m_layerMask))
        {
            m_collider2D.isTrigger = true;
        }
        // 아무것도 맞지 않으면 트리거를 끔
        else
        {
            m_collider2D.isTrigger = false;
        }
    }
}
