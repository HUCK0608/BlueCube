using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckPoint : MonoBehaviour
{
    /// <summary>저장 위치</summary>
    private Vector3 m_savePosition;
    public Vector3 GetSavePosition { get { return m_savePosition; } }

    private void Awake()
    {
        m_savePosition = transform.position;
    }

    /// <summary>체크포인트 등록</summary>
    public void RegisterCheckPoint()
    {
        CheckPointManager.Instance.SetCheckPoint(this);
        gameObject.SetActive(false);
    }
}
