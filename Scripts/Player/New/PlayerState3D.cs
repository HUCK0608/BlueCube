using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D : PlayerState
{
    protected PlayerController3D m_subController;

    protected override void Awake()
    {
        base.Awake();

        m_subController = GetComponent<PlayerController3D>();
    }

    public override void InitState()
    {
        Debug.Log("3D 상태 진입 : " + this.GetType());
    }

    protected virtual void Update()
    {
        if (GameLibrary.Bool_IsCO)
            return;
    }

    public override void EndState()
    {
        Debug.Log("3D 상태 종료 : " + this.GetType());
    }
}
