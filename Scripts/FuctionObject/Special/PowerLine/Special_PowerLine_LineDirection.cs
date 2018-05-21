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

    private List<Vector3> m_lineDirection;
    /// <summary>라인이 나아가있는 방향</summary>
    public List<Vector3> LineDirection { get { return m_lineDirection; } }

    private int m_lineDirectionCount;
    /// <summary>활성화 된 방향 개수</summary>
    public int LineDirectionCount { get { return m_lineDirectionCount; } }

    private void Awake()
    {
        InitDirection();
    }

    /// <summary>활성화 된 방향 초기화</summary>
    private void InitDirection()
    {
        m_lineDirection = new List<Vector3>();

        if (m_isUp)
            m_lineDirection.Add(Vector3.up);
        if (m_isRight)
            m_lineDirection.Add(Vector3.right);
        if (m_isLeft)
            m_lineDirection.Add(Vector3.left);
        if (m_isDown)
            m_lineDirection.Add(Vector3.down);

        m_lineDirectionCount = m_lineDirection.Count;
    }

    /// <summary>해당 방향에 라인이 있으면 true를 반환</summary>
    public bool IsHaveLine(Vector3 direction)
    {
        return m_lineDirection.Contains(direction);
    }
}
