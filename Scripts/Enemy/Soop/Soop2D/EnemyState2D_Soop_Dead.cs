using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState2D_Soop_Dead : EnemyState2D_Soop
{
    public override void InitState()
    {
        base.InitState();

        m_mainController.DeadLogic();
    }
}
