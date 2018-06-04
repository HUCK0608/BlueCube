using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Pushing : PlayerState3D
{
    [SerializeField]
    private GameObject m_pushingEffect;

    /// <summary>Pushing 모션이 끝났을 경우 true를 반환</summary>
    bool m_isEndPushingMotion;

    public override void InitState()
    {
        base.InitState();

        m_isEndPushingMotion = false;
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 이동 및 모션이 끝났을 경우 Idle 상태로 변경
        if (m_isEndPushingMotion && !m_playerManager.Hand.CurrentPushItem.IsMove)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    /// <summary>오브젝트를 민다 (애니메이션 이벤트에서 호출)</summary>
    public void PushObject()
    {
        m_pushingEffect.SetActive(true);
        m_playerManager.Hand.CurrentPushItem.PushObject();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 호출)</summary>
    public void CompletePushingMotion()
    {
        m_isEndPushingMotion = true;
    }

    public override void EndState()
    {
        base.EndState();

        m_pushingEffect.SetActive(false);
    }
}
