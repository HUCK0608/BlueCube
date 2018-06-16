using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D_Dead : PlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        m_subController.MoveStopAll();
    }
}
