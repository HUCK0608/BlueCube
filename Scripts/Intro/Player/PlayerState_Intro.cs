using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Intro : MonoBehaviour
{
    /// <summary>컨트롤러</summary>
    protected PlayerController_Intro m_controller;

    private void Awake()
    {
        m_controller = GetComponentInParent<PlayerController_Intro>();
    }

    public virtual void InitState() { }
    public virtual void EndState() { }
}
