using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LightManager : MonoBehaviour
{
    private Light m_light;

    private static LightShadows m_noShadows = LightShadows.None;
    private static LightShadows m_softShadows = LightShadows.Soft;

    private void Awake()
    {
        m_light = GetComponent<Light>();
    }

    // 그림자 활성화
    public void ShadowEnable(bool value)
    {
        if (value)
            m_light.shadows = m_softShadows;
        else
            m_light.shadows = m_noShadows;
    }

}
