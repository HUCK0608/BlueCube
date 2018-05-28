using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Dead : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        UIManager.Instance.DeadUI.SetActive(true);
    }

    private void Update()
    {
        m_subController.MoveStopXZ();
    }
}
