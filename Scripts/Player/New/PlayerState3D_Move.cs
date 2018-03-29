using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Move : PlayerState3D
{
    private Vector3 m_moveDirection;
    private Vector3 m_mouseDirectionToPlayer;

    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // 시점변환중이거나 탐지시점이면 return
        if (GameLibrary.Bool_IsCO)
            return;

        // 방향키 입력 방향을 가져옴
        m_moveDirection = m_subController.GetMoveDirection();
        // 플레이어에서 마우스의 방향을 가져옴
        m_mouseDirectionToPlayer = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

        m_subController.Move(m_mouseDirectionToPlayer, m_moveDirection);
        Rotate();
        CheckIdleState();
        CheckAttackState();
        ChangeJumpUpState();
    }

    // 머리와 몸 회전
    private void Rotate()
    {
        // 정면 이동일 경우 머리는 마우스방향을 바라보고 몸은 이동방향으로 바라봄
        if(m_subController.MoveDirection.Equals(0))
        {
            m_subController.RotateHead(m_mouseDirectionToPlayer);
            m_subController.RotateBody(m_moveDirection);
        }
        // 정면 이동이 아닐경우 마우스 방향을 바라봄
        else
        {
            m_subController.RotateHeadAndBody(m_mouseDirectionToPlayer);
        }
    }

    // Idle 상태로 바뀔지 체크
    private void CheckIdleState()
    {
        if (m_moveDirection.Equals(Vector3.zero))
            m_mainController.ChangeState3D(E_PlayerState.Idle);
    }

    // Attack 상태로 바뀔지 체크
    private void CheckAttackState()
    {
        // 공격키를 눌렀을 때
        if (Input.GetKeyDown(m_playerManager.Stat.AttackKey))
            // 무기를 사용할 수 있다면 Attack 상태로 변경
            if (m_playerManager.PlayerWeapon.CanUse)
                m_mainController.ChangeState3D(E_PlayerState.Attack);
    }

    public override void EndState()
    {
        base.EndState();

        m_subController.MoveStop();
    }
}
