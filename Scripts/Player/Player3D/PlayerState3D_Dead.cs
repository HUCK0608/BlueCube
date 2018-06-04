using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Dead : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        m_subController.MoveStopXZ();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 사용)</summary>
    public void CompleteDeadMotion()
    {
        UIManager.Instance.DeadUI.SetActive(true);
    }
}
