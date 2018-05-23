using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_ChangeView : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        // 시점변환 실행
        m_playerManager.Skill.ChangeView();
    }

    private void Update()
    {
    }

    public override void EndState()
    {
        base.EndState();
    }
}
