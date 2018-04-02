using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Idle : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        // 모든 속도 멈춤
        m_subController.MoveStopAll();
    }

    private void Update()
    {        
        // x, z 이동을 멈춤
        m_subController.MoveStopXZ();

        if (GameLibrary.Bool_IsPlayerStop)
        {
            // 중력만 적용
            m_subController.ApplyGravity();
            return;
        }

        // 플레이어에서 마우스의 방향을 가져옴
        Vector3 mouseDirectionToPlayer = CameraManager.Instance.GetMouseDirectionToPivot(transform.position);

        // 머리, 몸 회전
        m_subController.RotateHeadAndBody(mouseDirectionToPlayer);

        // 중력적용
        m_subController.ApplyGravity();

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 키 입력에 따른 이동 방향 벡터를 가져옴
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 상호작용 키를 눌렀을 때
        if (Input.GetKeyDown(m_playerManager.Stat.InteractionKey))
        {
            Vector3 rayOrigin = transform.position + Vector3.up;
            RaycastHit hit;
            int pickItemLayerMask = (1 << 9);
            int pushItemLayerMask = (1 << 10);

            // 바라보는 방향에 들 수 있는 아이템이 있으면 PickInit 상태로 변경
            if (GameLibrary.Raycast3D(rayOrigin, m_subController.Head.forward, out hit, m_playerManager.Stat.ItemCheckDistance, pickItemLayerMask))
            {
                // 아이템 스크립트를 저장
                m_playerManager.Hand.CurrentPickItem = hit.transform.GetComponent<Item_PickPut>();

                // 상태 변경
                m_mainController.ChangeState3D(E_PlayerState3D.PickInit);
            }
            // 바라보는 방향에 밀 수 있는 아이템이 있으면 PushInit 상태로 변경
            else if (GameLibrary.Raycast3D(rayOrigin, m_subController.Head.forward, out hit, m_playerManager.Stat.ItemCheckDistance, pushItemLayerMask))
            {
                // 아이템 스크립트를 저장
                m_playerManager.Hand.CurrentPushItem = hit.transform.GetComponent<Item_Push>();

                // 상태 변경
                m_mainController.ChangeState3D(E_PlayerState3D.PushInit);
            }
        }
        // 공격키를 눌렀을 때 무기가 사용 가능하고 땅에 있다면 Attack 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.AttackKey))
        {
            if (m_playerManager.Weapon.CanUse && m_playerManager.IsGrounded)
                m_mainController.ChangeState3D(E_PlayerState3D.Attack);
        }
        // 점프키를 눌렀을 때 땅에 있으면 JumpUp 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState3D(E_PlayerState3D.JumpUp);
        }
        // 이동 입력이 있을 경우 Move 상태로 변경
        else if (!moveDirection.Equals(Vector3.zero))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Move);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
