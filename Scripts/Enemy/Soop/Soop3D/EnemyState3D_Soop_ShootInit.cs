using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState3D_Soop_ShootInit : EnemyState3D_Soop
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    private void ChangeStates()
    {

    }

    public override void EndState()
    {
        base.EndState();
    }
}
