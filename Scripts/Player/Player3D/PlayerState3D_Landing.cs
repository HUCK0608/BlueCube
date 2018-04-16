using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Landing : PlayerState3D
{
    // 착지 모션이 끝났는지 여부
    private bool m_isEndLandingMotion;

    public override void InitState()
    {
        base.InitState();

        m_isEndLandingMotion = false;
    }

    private void Update()
    {
        // 이동방향 가져오기
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotation(moveDirection, m_playerManager.Stat.MoveSpeed_Jump);

        // 중력 적용
        m_subController.ApplyGravity();

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 점프 키를 누를 경우 JumpUp 상태로 변경
        if(Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.JumpUp);
        }
        // 착지 모션이 끝날 경우 Idle 상태로 변경
        else if(m_isEndLandingMotion)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }

    // 착지 애니메이션 마지막 프레임에 착지가 완료됬다고 설정
    public void CompleteLandingMotion()
    {
        m_isEndLandingMotion = true;
    }
}
