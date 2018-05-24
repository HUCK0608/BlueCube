using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickInit : PlayerState3D
{
    // 들기 애니메이션이 끝났는지 체크하는 변수
    private bool m_isEndPickInitMotion;

    public override void InitState()
    {
        base.InitState();

        m_isEndPickInitMotion = false;

        // 모든 속도를 멈춤
        m_subController.MoveStopAll();
    }

    private void Update()
    {
        // 상태 변경
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 들기 애니메이션이 끝나고 오브젝트 들기가 완료됬을 경우 PickIdle 상태로 변경
        if (m_isEndPickInitMotion && m_playerManager.Hand.CurrentPickPutObject.IsPick)
            m_mainController.ChangeState3D(E_PlayerState3D.PickIdle);
    }

    public override void EndState()
    {
        base.EndState();
    }

    /// <summary>오브젝트 들기</summary>
    public void PickObject()
    {
        m_playerManager.Hand.CurrentPickPutObject.PickObject();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 호출)</summary>
    public void CompletePickInitMotion()
    {
        m_isEndPickInitMotion = true;
    }
}
