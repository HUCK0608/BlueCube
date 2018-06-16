using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D : PlayerState
{
    private static string m_animatorParametersName = "CurrentState";

    protected PlayerController2D m_subController;

    protected override void Awake()
    {
        base.Awake();

        m_subController = GetComponent<PlayerController2D>();
    }

    public override void InitState()
    {
        m_subController.Animator.SetInteger(m_animatorParametersName, (int)m_mainController.CurrentState2D);
        //Debug.Log("2D 상태 진입 : " + this.GetType());
    }

    public override void EndState()
    {
        //Debug.Log("2D 상태 종료 : " + this.GetType());
    }
}
