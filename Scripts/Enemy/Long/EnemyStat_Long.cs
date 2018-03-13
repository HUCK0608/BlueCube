using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyStat_Long : EnemyStat
{
    [SerializeField]
    private float m_reloadTime;
    public float ReloadTime { get { return m_reloadTime; } }
}
