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

        // 중력적용
        m_subController.ApplyGravity();

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 키 입력에 따른 이동 방향 벡터를 가져옴
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 시점변환 키를 눌렀을 때 시점변환 실행
        if(Input.GetKeyDown(m_playerManager.Stat.ChangeViewKey))
        {
            m_playerManager.Skill.ChangeView();
        }
        // 상호작용 키를 눌렀을 때
        else if (Input.GetKeyDown(m_playerManager.Stat.InteractionKey))
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
            else if (GameLibrary.Raycast3D(rayOrigin, m_subController.Forward, out hit, m_playerManager.Stat.ItemCheckDistance, pushItemLayerMask))
            {
                // 아이템 스크립트를 저장
                m_playerManager.Hand.CurrentPushItem = hit.transform.GetComponentInParent<Interaction_Push>();

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
