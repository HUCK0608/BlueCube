using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D : PlayerState
{
    private static string m_animatorParametersName = "CurrentState";

    /// <summary>3D 컨트롤러</summary>
    protected PlayerController3D m_subController;

    protected override void Awake()
    {
        base.Awake();

        m_subController = GetComponent<PlayerController3D>();
    }

    /// <summary>상태 진입시 실행</summary>
    public override void InitState()
    {
        m_subController.Animator.SetInteger(m_animatorParametersName, (int)m_mainController.CurrentState3D);
        //Debug.Log("3D 상태 진입 : " + this.GetType());
    }

    /// <summary>상태 변경 모음</summary>
    protected virtual void ChangeStates() { }

    /// <summary>상태 종료시 실행</summary>
    public override void EndState()
    {
        //Debug.Log("3D 상태 종료 : " + this.GetType());
    }
}
