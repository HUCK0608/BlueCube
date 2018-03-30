using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PushIdle : PlayerState3D
{
    // 현재 상호작용중인 밀기 아이템
    private Item_Push m_currentPushItem;

    // 아이템쪽으로의 방향
    private Vector3 m_directionToItem;
    private float m_addTime;

    public override void InitState()
    {
        base.InitState();

        m_currentPushItem = m_playerManager.Hand.CurrentPushItem;

        // 누적시간 초기화
        m_addTime = 0f;

        // 플레이어에서 아이템으로의 방향
        m_directionToItem = m_currentPushItem.transform.position - transform.position;
        m_directionToItem.y = 0f;
        m_directionToItem =  m_directionToItem.normalized;
    }
    
    private void Update()
    {
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 상호작용 키를 누를경우 Idle 상태로 변경
        if(Input.GetKeyDown(m_playerManager.Stat.InteractionKey))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
        // 이동입력을 받을 때 미는 방향이 일치하고 밀기 허용시간만큼 밀기를 적용하면 PushMove 상태로 변경
        else if(!moveDirection.Equals(Vector3.zero) && Vector3.Angle(moveDirection, m_directionToItem).Equals(0f))
        {
            m_addTime += Time.deltaTime;

            // 밀기 허용시간을 넘어가면 실행
            if (m_addTime >= m_playerManager.Stat.PushActivateTime)
            {
                // 밀 방향에 아무것도 없는지 체크해서 아무것도 없다면 밀기 실행
                if (m_currentPushItem.IsCanMoveToDirection(moveDirection))
                    m_mainController.ChangeState3D(E_PlayerState3D.PushMove);
                else
                    m_addTime = 0f;
            }
        }
        // 이동입력이 없거나 미는 방향이 일치하지 않을경우 누적시간 초기화
        else if (moveDirection.Equals(Vector3.zero) || !m_directionToItem.Equals(moveDirection))
        {
            if (!m_addTime.Equals(0f))
                m_addTime = 0f;
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
