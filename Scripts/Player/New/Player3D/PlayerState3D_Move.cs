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
        // 시점변환중이거나 탐지시점이면 x, z속도를 멈추고 중력적용
        if (GameLibrary.Bool_IsCO)
        {
            m_subController.MoveStopXZ();

            // 중력적용
            m_subController.ApplyGravity();
        }
        // 아닐경우 아래 함수를 진행
        else
        {
            // 방향키 입력 방향을 가져옴
            m_moveDirection = m_subController.GetMoveDirection();
            // 플레이어에서 마우스의 방향을 가져옴
            m_mouseDirectionToPlayer = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

            // 이동 및 회전
            m_subController.Move(m_mouseDirectionToPlayer, m_moveDirection);
            Rotate();

            // 중력적용
            m_subController.ApplyGravity();

            ChangeIdleState();
            ChangeAttackState();
            ChangeJumpUpState();
            ChangeLadderInitState();
        }
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
    private void ChangeIdleState()
    {
        // 이동입력이 없으면 Idle 상태로 변경
        if (m_moveDirection.Equals(Vector3.zero))
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);

        // 시점변환중이거나 관찰시점이면 Idle 상태로 변경
        if (GameLibrary.Bool_IsCO)
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
    }

    // Attack 상태로 바뀔지 체크
    private void ChangeAttackState()
    {
        // 공격키를 눌렀을 때
        if (Input.GetKeyDown(m_playerManager.Stat.AttackKey))
            // 무기를 사용할 수 있다면 Attack 상태로 변경
            if (m_playerManager.PlayerWeapon.CanUse)
                m_mainController.ChangeState3D(E_PlayerState3D.Attack);
    }

    // JumpUp 상태로 바뀔지 체크
    private void ChangeJumpUpState()
    {
        // 점프키를 눌렀을 때
        if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
            // 땅에 있다면 JumpUp 상태로 변경
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState3D(E_PlayerState3D.JumpUp);
    }

    // LadderInit 상태로 바뀔지 체크
    private void ChangeLadderInitState()
    {
        // 이동방향에 사다리가 있으면 LadderInit 상태로 변경
        if(m_subController.CheckLadder.IsOnLadder(m_moveDirection))
        {
            // 해당 이동 방향에 있는 곳에 레이를 쏴서 사다리 스크립트를 저장함
            m_subController.CurrentLadder = m_subController.CheckLadder.GetLadder(m_moveDirection);

            // LadderInit 상태로 변경
            m_mainController.ChangeState3D(E_PlayerState3D.LadderInit);
        }
    }

    public override void EndState()
    {
        base.EndState();

        // x, z속도를 멈춤
        m_subController.MoveStopXZ();
    }
}
