using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_OnOff : MonoBehaviour
{
    // 스위치가 켜졌는지
    private bool m_isOn;
    public bool IsOn { get { return m_isOn; } }

    public void On()
    {
        m_isOn = true;
    }
}
