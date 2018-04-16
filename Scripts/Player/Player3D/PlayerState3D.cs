using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D : PlayerState
{
    private Animator m_animator;
    private static string m_animatorParametersName = "CurrentState";

    protected PlayerController3D m_subController;

    protected override void Awake()
    {
        base.Awake();

        m_animator = GetComponent<Animator>();

        m_subController = GetComponent<PlayerController3D>();
    }

    public override void InitState()
    {
        m_animator.SetInteger(m_animatorParametersName, (int)m_mainController.CurrentState3D);
        //Debug.Log("3D 상태 진입 : " + this.GetType());
    }

    public override void EndState()
    {
        //Debug.Log("3D 상태 종료 : " + this.GetType());
    }
}
