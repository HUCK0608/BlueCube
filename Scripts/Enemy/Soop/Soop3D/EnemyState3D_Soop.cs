using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState3D_Soop : EnemyState
{
    /// <summary>메인 컨트롤러</summary>
    protected EnemyController_Soop m_mainController;
    /// <summary>서브 컨트롤러</summary>
    protected EnemyController3D_Soop m_subController;

    protected virtual void Awake()
    {
        m_mainController = GetComponentInParent<EnemyController_Soop>();
        m_subController = GetComponent<EnemyController3D_Soop>();
    }
}
