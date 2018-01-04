using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OutZone : MonoBehaviour
{
    private Vector3 m_respawnPoint;
    public Vector3 RespawnPoint { get { return m_respawnPoint; } }

    private void Awake()
    {
        m_respawnPoint = transform.Find("RespawnPoint").position;
    }
}
