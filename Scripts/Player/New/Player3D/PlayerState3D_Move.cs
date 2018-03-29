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
        // 방향키 입력 방향을 가져옴
        m_moveDirection = m_subController.GetMoveDirection();
        // 플레이어에서 마우스의 방향을 가져옴
        m_mouseDirectionToPlayer = CameraManager.Instance.GetMouseDirectionToPivot(transform.position);

        // 이동 및 회전
        m_subController.Move(m_mouseDirectionToPlayer, m_moveDirection);
        Rotate();

        // 중력적용
        m_subController.ApplyGravity();

        // 상태 변경
        ChangeStates();
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

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 아이템 들기를 눌렀을 때 몸의 정면방향에 들 수 있는 아이템이 있을 경우 PickInit 상태로 변경
        if (Input.GetKeyDown(m_playerManager.Stat.PickItemKey))
        {
            int layerMask = (1 << 9);
            Vector3 rayOrigin = transform.position + Vector3.up;
            RaycastHit hit;

            // 바라보는 방향으로 레이를 쏨
            if (GameLibrary.Raycast3D(rayOrigin, m_subController.Head.forward, out hit, m_playerManager.Stat.ItemCheckDistance, layerMask))
            {
                // 아이템 스크립트를 저장시킴
                m_playerManager.Hand.CurrentPickItem = hit.transform.GetComponent<Item_PickPut>();

                // 상태 변경
                m_mainController.ChangeState3D(E_PlayerState3D.PickInit);
            }
        }
        // 이동방향에 사다리가 있으면 LadderInit 상태로 변경
        else if (m_subController.CheckLadder.IsOnLadder(m_moveDirection))
        {
            // 해당 이동 방향에 있는 곳에 레이를 쏴서 사다리 스크립트를 저장함
            m_subController.CurrentLadder = m_subController.CheckLadder.GetLadder(m_moveDirection);

            // LadderInit 상태로 변경
            m_mainController.ChangeState3D(E_PlayerState3D.LadderInit);
        }
        // 점프키를 눌렀을 때 땅에 있으면 Jump 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState3D(E_PlayerState3D.JumpUp);
        }
        // 공격키를 눌렀을 때 무기가 사용 가능하면 Attack 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.AttackKey))
        {
            if (m_playerManager.Weapon.CanUse)
                m_mainController.ChangeState3D(E_PlayerState3D.Attack);
        }
        // 이동 입력이 없거나 플레이어가 멈춰야 하는 상황이면 Idle 상태로 변경
        else if (m_moveDirection.Equals(Vector3.zero) || GameLibrary.Bool_IsPlayerStop)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
