using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_OnOff_Stepping2D : MonoBehaviour
{
    private Switch_OnOff_Stepping m_switch;

    private void Awake()
    {
        m_switch = GetComponentInParent<Switch_OnOff_Stepping>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_switch.SwitchOn();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_switch.SwitchOff();
        }
    }
}
