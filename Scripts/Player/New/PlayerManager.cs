using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    private PlayerStat m_stat;
    /// <summary>플레이어 스탯</summary>
    public PlayerStat Stat { get { return m_stat; } }

    private PlayerWeapon m_playerWeapon;
    /// <summary>플레이어 무기</summary>
    public PlayerWeapon PlayerWeapon { get { return m_playerWeapon; } }

    private GameObject m_player3D_Object;

    public Vector3 PlayerPosition { get { return m_player3D_Object.transform.position; } }
    private void Awake()
    {
        m_stat = GetComponent<PlayerStat>();
        m_playerWeapon = GetComponent<PlayerWeapon>();

        m_player3D_Object = transform.Find("Player3D").gameObject;
    }
}
