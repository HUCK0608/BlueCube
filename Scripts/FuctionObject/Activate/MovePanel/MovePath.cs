using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovePath : MonoBehaviour
{
    // 경로들
    private List<Transform> m_paths;

    // 경로 개수
    private int m_pathCount;
    public int PathCount { get { return m_pathCount; } }

    private void Awake()
    {
        m_paths = new List<Transform>();

        m_pathCount = transform.childCount;

        for (int i = 0; i < m_pathCount; i++)
            m_paths.Add(transform.GetChild(i));
    }

    private void OnDrawGizmos()
    {
        m_paths = new List<Transform>();

        m_pathCount = transform.childCount;

        for (int i = 0; i < m_pathCount; i++)
            m_paths.Add(transform.GetChild(i));

        Color defalutColor = Gizmos.color;
        float sphereRadius = 0.5f;

        for(int i = 0; i < m_pathCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_paths[i].position, sphereRadius);

            Gizmos.color = Color.yellow;
            // 현재 경로가 마지막 경로라면 현재 경로랑 처음 경로랑 이어지는 선을 그려줌
            if(i.Equals(m_pathCount - 1))
            {
                Gizmos.DrawLine(m_paths[i].position, m_paths[0].position);
            }
            // 마지막 경로가 아닐경우 다음 경로랑 이어지는 선을 그려줌
            else
            {
                Gizmos.DrawLine(m_paths[i].position, m_paths[i + 1].position);
            }
        }

        Gizmos.color = defalutColor;
    }

    // 경로 위치 반환
    public Vector3 PathPosition(int number)
    {
        return m_paths[number].position;
    }
}
