using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Move : PlayerState3D
{
    private Vector3 m_moveDirection;

    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // 방향키 입력 방향을 가져옴
        m_moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotation(m_moveDirection, m_playerManager.Stat.MoveSpeed_Forward);

        // 중력적용
        m_subController.ApplyGravity();

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 상호작용 키를 눌렀을 때
        if (Input.GetKeyDown(m_playerManager.Stat.InteractionKey))
        {
            Vector3 rayOrigin = transform.position + Vector3.up;
            RaycastHit hit;
            int pickItemLayerMask = (1 << 9);
            int pushItemLayerMask = (1 << 10);
            int hintItemLayerMask = (1 << 13);

            // 바라보는 방향에 들 수 있는 아이템이 있으면 PickInit 상태로 변경
            if (GameLibrary.Raycast3D(rayOrigin, m_subController.Forward, out hit, m_playerManager.Stat.ItemCheckDistance, pickItemLayerMask))
            {
                // 아이템 스크립트를 저장
                m_playerManager.Hand.CurrentPickItem = hit.transform.GetComponent<Item_PickPut>();

                // 상태 변경
                m_mainController.ChangeState3D(E_PlayerState3D.PickInit);
            }
            // 바라보는 방향에 밀 수 있는 아이템이 있으면 PushInit 상태로 변경
            else if(GameLibrary.Raycast3D(rayOrigin, m_subController.Forward, out hit, m_playerManager.Stat.ItemCheckDistance, pushItemLayerMask))
            {
                // 아이템 스크립트를 저장
                m_playerManager.Hand.CurrentPushItem = hit.transform.GetComponent<Item_Push>();

                // 상태 변경
                m_mainController.ChangeState3D(E_PlayerState3D.PushInit);
            }
            // 바라보는 방향에 힌트 아이템이 있을경우 힌트를 표시
            else if (GameLibrary.Raycast3D(rayOrigin, m_subController.Forward, out hit, m_playerManager.Stat.ItemCheckDistance, hintItemLayerMask))
            {
                // 힌트를 표시
                hit.transform.GetComponent<Item_HintObject>().ShowHint();
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
