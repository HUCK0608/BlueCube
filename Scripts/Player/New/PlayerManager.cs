using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    private static PlayerManager m_instance;
    public static PlayerManager Instance { get { return m_instance; } }

    private PlayerStat m_stat;
    /// <summary>플레이어 스탯</summary>
    public PlayerStat Stat { get { return m_stat; } }

    private PlayerSkill_ChangeView m_playerSkill_ChangeView;

    private PlayerWeapon m_playerWeapon;
    /// <summary>플레이어 무기</summary>
    public PlayerWeapon PlayerWeapon { get { return m_playerWeapon; } }

    private GameObject m_player3D_Object;
    /// <summary>플레이어3D 오브젝트를 반환</summary>
    public GameObject Player3D_Object { get { return m_player3D_Object; } }

    // 다른곳에서 플레이어 관련 속성을 편하게 가져가기 위해 만든 변수
    // 시점변환 관련
    /// <summary>현재 시점을 반환 (View2D, View3D)</summary>
    public E_ViewType CurrentView { get { return m_playerSkill_ChangeView.CurrentView; } }
    /// <summary>현재 시점변환 중일경우 true를 반환</summary>
    public bool IsViewChange { get { return m_playerSkill_ChangeView.IsViewChange; } }

    private void Awake()
    {
        m_instance = this;

        m_stat = GetComponent<PlayerStat>();
        m_playerSkill_ChangeView = GetComponent<PlayerSkill_ChangeView>();
        m_playerWeapon = GetComponent<PlayerWeapon>();

        m_player3D_Object = transform.Find("Player3D").gameObject;
    }
}
