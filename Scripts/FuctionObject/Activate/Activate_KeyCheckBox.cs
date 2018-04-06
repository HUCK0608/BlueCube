using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Activate_KeyCheckBox : MonoBehaviour
{
    private Activate_Key m_activate;

    private void Awake()
    {
        m_activate = GetComponentInParent<Activate_Key>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_activate.KeyCheck();
        }
    }
}
