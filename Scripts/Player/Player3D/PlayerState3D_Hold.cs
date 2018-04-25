using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Hold : PlayerState3D
{
    float m_addTime;

    public override void InitState()
    {
        base.InitState();

        m_addTime = 0f;
    }

    private void Update()
    {
        if (PlayerManager.Instance.IsViewChange || PlayerManager.Instance.IsViewChangeReady)
            return;

        m_addTime += Time.deltaTime;

        ChangeStates();
    }

    private void ChangeStates()
    {
        // 최대로 홀딩 될 수 있는 시간이 지나면 Idle 상태로 변경
        if(m_addTime >= m_playerManager.Stat.MaxHoldTime)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
        // 시점변환 키를 눌렀을 때 시점변환 실행
        else if(Input.GetKeyDown(m_playerManager.Stat.ChangeViewKey))
        {
            m_playerManager.Skill.ChangeView();
        }
        // 이동 입력이 있으면 Move 상태로 변경
        else if(!m_subController.GetMoveDirection().Equals(Vector3.zero))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Move);
        }
    }
    
    public override void EndState()
    {
        base.EndState();
    }
}
