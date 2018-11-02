using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckPoint : MonoBehaviour
{
    /// <summary>저장 위치</summary>
    private Vector3 m_savePosition;
    public Vector3 GetSavePosition { get { return m_savePosition; } }

    private float m_colliderSizeZ;

    private void Awake()
    {
        m_savePosition = transform.position;

        m_colliderSizeZ = GetComponentInChildren<BoxCollider>().size.z;
    }

    /// <summary>체크포인트 등록</summary>
    public void RegisterCheckPoint()
    {
        float playerPositionZ = PlayerManager.Instance.Player3D_Object.transform.position.z;

        if (playerPositionZ >= transform.position.z - m_colliderSizeZ * transform.localScale.z && playerPositionZ <= transform.position.z + m_colliderSizeZ * transform.localScale.z)
        {
            CheckPointManager.Instance.SetCheckPoint(this);
            gameObject.SetActive(false);
        }
    }
}
