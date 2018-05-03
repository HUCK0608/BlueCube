using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorgiState_Intro : MonoBehaviour
{
    protected CorgiController_Intro m_controller;

    private void Awake()
    {
        m_controller = GetComponentInParent<CorgiController_Intro>();
    }

    public virtual void InitState() { }
    public virtual void EndState() { }
}
