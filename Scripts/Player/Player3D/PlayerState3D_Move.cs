using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Move : PlayerState3D
{
    private Vector3 m_moveDirection;

    public override void InitState()
    {
        base.InitState();

        // 먼지 이펙트 실행
        m_playerManager.DustEffectParticle.Play();
    }

    private void Update()
    {
        // 방향키 입력 방향을 가져옴
        m_moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotate(m_moveDirection);

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 밑에 아무것도 없다면 Falling 상태로 변경
        if (!m_mainController.IsGrounded)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Falling);
        }
        // 시점변환 키를 눌렀을 때 시점변환이 가능하면 Idle 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.ChangeViewKey))
        {
            if (m_playerManager.Skill.ChangeView())
                m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
        // 상호작용 키를 눌렀을 때
        else if (Input.GetKeyDown(m_playerManager.Stat.InteractionKey))
        {
            List<Transform> itemCheckPoints = m_subController.ItemCheckPoints;
            int itemCheckPointCount = itemCheckPoints.Count;
            float itemCheckDistance = m_playerManager.Stat.ItemCheckDistance;
            RaycastHit hit;

            // 들기 아이템
            for (int i = 0; i < itemCheckPointCount; i++)
            {
                if (GameLibrary.Raycast3D(itemCheckPoints[i].position, itemCheckPoints[i].forward, out hit, itemCheckDistance, GameLibrary.LayerMask_InteractionPickPut))
                {
                    m_playerManager.Hand.CurrentPickPutObject = hit.transform.GetComponentInParent<Interaction_PickPut>();

                    m_mainController.ChangeState3D(E_PlayerState3D.PickInit);

                    return;
                }
            }
            // 밀기 아이템
            for (int i = 0; i < itemCheckPointCount; i++)
            {
                if (GameLibrary.Raycast3D(itemCheckPoints[i].position, itemCheckPoints[i].forward, out hit, itemCheckDistance, GameLibrary.LayerMask_InteractionPush))
                {
                    m_playerManager.Hand.CurrentPushItem = hit.transform.GetComponentInParent<Interaction_Push>();

                    m_mainController.ChangeState3D(E_PlayerState3D.PushInit);

                    return;
                }
            }
            // 힌트 아이템
            for (int i = 0; i < itemCheckPointCount; i++)
            {
                if (GameLibrary.Raycast3D(itemCheckPoints[i].position, itemCheckPoints[i].forward, out hit, itemCheckDistance, GameLibrary.LayerMask_InteractionHint))
                {
                    hit.transform.GetComponent<Item_HintObject>().ShowHint();

                    return;
                }
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

        // 먼지 이펙트 정지
        m_playerManager.DustEffectParticle.Stop();
    }
}
