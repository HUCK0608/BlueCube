using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SoopTemp : MonoBehaviour
{
    private bool m_isEndShootInitMotion;
    public bool IsEndShootInitMotion { get { return m_isEndShootInitMotion; } set { m_isEndShootInitMotion = value; } }

    private bool m_isEndShootMotion;
    public bool IsEndShootMotion { get { return m_isEndShootMotion; } set { m_isEndShootMotion = value; } }

    public void CompleteShootInitMotion()
    {
        m_isEndShootInitMotion = true;
    }

    public void CompleteShootMotion()
    {
        m_isEndShootMotion = true;
    }
}
