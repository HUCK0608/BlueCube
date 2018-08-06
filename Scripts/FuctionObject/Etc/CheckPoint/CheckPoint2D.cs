using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckPoint2D : MonoBehaviour
{
    private CheckPoint m_checkPoint;

    private void Awake()
    {
        m_checkPoint = GetComponentInParent<CheckPoint>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(GameLibrary.String_Player))
            m_checkPoint.RegisterCheckPoint();
    }
}
