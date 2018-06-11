using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_ChangeViewIdle : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        throw new NotImplementedException();
    }

    public override void EndState()
    {
        base.EndState();
    }
}
