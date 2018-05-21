using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>라인이 어느방향에 있는지에 대한 클래스</summary>
[Serializable]
public sealed class Special_PowerLine_LineDirection : MonoBehaviour
{
    [SerializeField]
    private bool m_isUp;
    [SerializeField]
    private bool m_isRight;
    [SerializeField]
    private bool m_isLeft;
    [SerializeField]
    private bool m_isDown;

    private List<Vector3> m_isOnDirection;
    /// <summary>활성화 된 3D방향</summary>
    public List<Vector3> IsOnDirection { get { return m_isOnDirection; } }

    private int m_isOnDirectionCount;
    /// <summary>활성화 된 방향 개수</summary>
    public int IsOnDirectionCount { get { return m_isOnDirectionCount; } }

    private void Awake()
    {
        InitDirection();
    }

    /// <summary>활성화 된 방향 초기화</summary>
    private void InitDirection()
    {
        m_isOnDirection = new List<Vector3>();

        if (m_isUp)
            m_isOnDirection.Add(Vector3.up);
        if (m_isRight)
            m_isOnDirection.Add(Vector3.right);
        if (m_isLeft)
            m_isOnDirection.Add(Vector3.left);
        if (m_isDown)
            m_isOnDirection.Add(Vector3.down);

        m_isOnDirectionCount = m_isOnDirection.Count;
    }
}
