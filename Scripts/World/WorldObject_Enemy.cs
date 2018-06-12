using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Enemy : WorldObject
{
    [Header("Don't Touch!")]
    [SerializeField]
    private EnemyController m_enemyController;

    public override void Change3D()
    {
        m_enemyController.ChangeEnemy3D();
    }

    public override void Change2D()
    {
        m_enemyController.ChangeEnemy2D();
    }
}
