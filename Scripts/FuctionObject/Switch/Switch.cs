using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    /// <summary>스위치가 켜져있는지 여부</summary>
    protected bool m_isOn;
    public bool IsOn { get { return m_isOn; } }
}
