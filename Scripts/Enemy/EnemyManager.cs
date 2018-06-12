using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChangeEnemy3D();
public delegate void ChangeEnemy2D();

public sealed class EnemyManager : MonoBehaviour
{
    private static EnemyManager m_instance;
    public static EnemyManager Instance { get { return m_instance; } }

    public ChangeEnemy3D changeEnemy3D_Delegate;
    public ChangeEnemy2D changeEnemy2D_Delegate;

    private void Awake()
    {
        m_instance = this;
    }
}
