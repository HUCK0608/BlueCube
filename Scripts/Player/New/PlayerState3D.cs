using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D : PlayerState
{
    protected PlayerController3D m_subController;

    protected override void Awake()
    {
        base.Awake();

        m_subController = GetComponent<PlayerController3D>();
    }

    public override void InitState()
    {
        Debug.Log("3D 상태 진입 : " + this.GetType());
    }

    public override void EndState()
    {
        Debug.Log("3D 상태 종료 : " + this.GetType());
    }

    /// <summary>점프 키를 눌렀을 때 땅에 있을경우 JumpUp 상태로 변경</summary>
    protected void ChangeJumpUpState()
    {
        if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState3D(E_PlayerState.JumpUp);
    }
}
