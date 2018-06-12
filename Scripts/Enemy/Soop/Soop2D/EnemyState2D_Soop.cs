using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState2D_Soop : EnemyState
{
    private static string m_animatorParameterPath = "CurrentState";

    /// <summary>메인 컨트롤러</summary>
    protected EnemyController_Soop m_mainController;
    /// <summary>서브 컨트롤러</summary>
    protected EnemyController2D_Soop m_subController;

    protected virtual void Awake()
    {
        m_mainController = GetComponentInParent<EnemyController_Soop>();
        m_subController = GetComponent<EnemyController2D_Soop>();
    }

    public override void InitState()
    {
        m_subController.Animator.SetInteger(m_animatorParameterPath, (int)m_mainController.CurrentState2D);
    }
}
