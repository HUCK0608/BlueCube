using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderInit : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        m_subController.LookRotation(m_subController.CheckLadder.LatelyLadder.Forward);

        m_mainController.ChangeState3D(E_PlayerState3D.LadderIdle);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
