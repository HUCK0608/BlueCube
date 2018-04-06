using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RotationZPanel : MonoBehaviour
{
    [SerializeField]
    private float m_rotationSpeed;

    [SerializeField]
    private E_RotationDir m_rotationDirection;

    private Vector3 m_rotation;

    private void Awake()
    {
        m_rotation = Vector3.zero;

        m_rotation.z = m_rotationSpeed * -(int)m_rotationDirection;
    }

    private void Update()
    {
        if (GameLibrary.Bool_IsGameStop_Old)
            return;

        PanelRotate();
    }

    private void PanelRotate()
    {
        transform.eulerAngles += m_rotation * Time.deltaTime;
    }
}
