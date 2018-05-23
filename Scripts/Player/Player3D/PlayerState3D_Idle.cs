using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Idle : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {        
        // x, z 이동을 멈춤
        m_subController.MoveStopXZ();

        if (GameLibrary.Bool_IsPlayerStop)
            return;

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 키 입력에 따른 이동 방향 벡터를 가져옴
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 밑에 아무것도 없다면 Falling 상태로 변경
        if(!m_mainController.IsGrounded)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Falling);
        }
        // 시점변환 키를 눌렀을 때 ChangeView 상태로 변경
        else if(Input.GetKeyDown(m_playerManager.Stat.ChangeViewKey))
        {
            m_playerManager.Skill.ChangeView();
        }
        // 상호작용 키를 눌렀을 때
        else if (Input.GetKeyDown(m_playerManager.Stat.InteractionKey))
        {
            List<Transform> itemCheckPoints = m_subController.ItemCheckPoints;
            int itemCheckPointCount = itemCheckPoints.Count;
            float itemCheckDistance = m_playerManager.Stat.ItemCheckDistance;
            RaycastHit hit;

            // 들기 아이템
            for(int i = 0; i < itemCheckPointCount; i ++)
            {
                if(GameLibrary.Raycast3D(itemCheckPoints[i].position, itemCheckPoints[i].forward, out hit, itemCheckDistance, GameLibrary.LayerMask_InteractionPickPut))
                {
                    m_playerManager.Hand.CurrentPickPutObject = hit.transform.GetComponentInParent<Interaction_PickPut>();

                    m_mainController.ChangeState3D(E_PlayerState3D.PickInit);

                    return;
                }
            }
            // 밀기 아이템
            for(int i = 0; i < itemCheckPointCount; i++)
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
