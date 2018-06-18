using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public sealed class FogManager : MonoBehaviour
{
    private GlobalFog m_fog;

    private float m_defaultHeight;

    private void Awake()
    {
        m_fog = GetComponentInChildren<GlobalFog>();

        m_defaultHeight = m_fog.height;
    }

    private void Update()
    {
        m_fog.height = m_defaultHeight + CameraManager.Instance.transform.position.y;
    }
}
