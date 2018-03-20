using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    private PlayerStat m_stat;
    public PlayerStat Stat { get { return m_stat; } }

    private void Awake()
    {
        m_stat = GetComponent<PlayerStat>();
    }
}
