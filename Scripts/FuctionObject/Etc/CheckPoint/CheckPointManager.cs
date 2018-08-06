using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class CheckPointManager : MonoBehaviour
{
    private static CheckPointManager m_instance;
    public static CheckPointManager Instance { get { return m_instance; } }

    /// <summary>시작 지점</summary>
    [SerializeField]
    private CheckPoint m_startPoint;

    /// <summary>현재 등록된 체크포인트</summary>
    private CheckPoint m_currentCheckPoint;
    /// <summary>체크 포인트 설정</summary>
    public void SetCheckPoint(CheckPoint newCheckPoint)
    {
        if(m_currentCheckPoint != null)
        {
            if (m_currentCheckPoint.Equals(newCheckPoint))
                return;
        }

        m_currentCheckPoint = newCheckPoint;
    }

    /// <summary>체크 포인트 반환</summary>
    public CheckPoint GetCheckPoint()
    {
        if (m_currentCheckPoint == null)
            return m_startPoint;
        else
            return m_currentCheckPoint;
    }

    private void Awake()
    {
        m_instance = this;
    }
}
