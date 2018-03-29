using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_On_Stepping2D : MonoBehaviour
{
    private Switch_On m_switch;

    private void Awake()
    {
        m_switch = GetComponentInParent<Switch_On>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(GameLibrary.String_Player))
        {
            m_switch.SwitchOn();
        }
    }
}
