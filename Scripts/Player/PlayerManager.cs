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

    // 스킬
    private PlayerSkill_ChangeView m_skill;

    private PlayerHand m_hand;
    /// <summary>플레이어 손</summary>
    public PlayerHand Hand { get { return m_hand; } }

    private PlayerInventory m_inventory;
    /// <summary>인벤토리</summary>
    public PlayerInventory Inventory { get { return m_inventory; } }

    // 컨트롤러
    private PlayerController m_mainController;
    private PlayerController3D m_subController3D;
    private PlayerController2D m_subController2D;
    /// <summary>플레이어의 메인 컨트롤러</summary>
    public PlayerController MainController { get { return m_mainController; } }
    /// <summary>플레이어의 3D컨트롤러</summary>
    public PlayerController3D SubController3D { get { return m_subController3D; } }
    /// <summary>플레이어의 2D컨트롤러</summary>
    public PlayerController2D SubController2D { get { return m_subController2D; } }

    private GameObject m_player3D_Object;
    /// <summary>플레이어3D 오브젝트를 반환</summary>
    public GameObject Player3D_Object { get { return m_player3D_Object; } }

    private GameObject m_player2D_Object;
    /// <summary>플레이어2D 오브젝트를 반환</summary>
    public GameObject Player2D_Object { get { return m_player2D_Object; } }

    // 다른곳에서 플레이어 관련 속성을 편하게 가져가기 위해 만든 변수
    // 시점변환 관련
    /// <summary>현재 시점을 반환 (View2D, View3D)</summary>
    public E_ViewType CurrentView { get { return m_skill.CurrentView; } }
    /// <summary>현재 시점변환이 준비중일경우 true를 반환</summary>
    public bool IsViewChangeReady { get { return m_skill.IsViewChangeReady; } }
    /// <summary>현재 시점변환이 실행중일경우 true를 반환</summary>
    public bool IsViewChange { get { return m_skill.IsViewChange; } }
    /// <summary>플레이어가 땅에 있을경우 true를 반환</summary>
    public bool IsGrounded { get { return m_mainController.IsGrounded; } }

    private void Awake()
    {
        m_instance = this;

        m_stat = GetComponent<PlayerStat>();
        m_skill = GetComponent<PlayerSkill_ChangeView>();
        m_hand = GetComponent<PlayerHand>();
        m_inventory = GetComponent<PlayerInventory>();

        m_mainController = GetComponent<PlayerController>();
        m_subController3D = GetComponentInChildren<PlayerController3D>();
        m_subController2D = GetComponentInChildren<PlayerController2D>();

        m_player3D_Object = transform.Find("Player3D").gameObject;
        m_player2D_Object = transform.Find("Player2D").gameObject;
    }

    private void Start()
    {
        PlayerChange3D();
    }

    /// <summary>플레이어를 3D로 변경함</summary>
    public void PlayerChange3D()
    {
        // 2D플레이어 비활성화
        m_player2D_Object.SetActive(false);
        // 3D플레이어의 부모를 그룹의 루트로 변경
        m_player3D_Object.transform.parent = transform;
        // 2D플레이어의 부모를 3D플레이어로 변경
        m_player2D_Object.transform.parent = m_player3D_Object.transform;
        // 3D플레이어 활성화
        m_player3D_Object.SetActive(true);
    }

    /// <summary>플레이어를 2D로 변경함</summary>
    public void PlayerChange2D()
    {
        // 3D플레이어 비활성화
        m_player3D_Object.SetActive(false);
        // 2D플레이어의 부모를 그룹의 루트로 변경
        m_player2D_Object.transform.parent = transform;
        // 3D플레이어의 부모를 2D플레이어로 변경
        m_player3D_Object.transform.parent = m_player2D_Object.transform;
        // 2D플레이어 활성화
        m_player2D_Object.SetActive(true);
    }

    // 체력감소 부분
    /// <summary>체력이 0이하일 경우 true를 반환</summary>
    private bool DieCheck()
    {
        return m_stat.Hp <= 0 ? true : false;
    }

    /// <summary>플레이어에게 데미지를 입힌다</summary>
    public void Hit(int damage)
    {
        m_stat.DecreaseHp(damage);
        Debug.Log("플레이어 데미지! 남은체력 : " + m_stat.Hp);
    }

    /// <summar>플레이어를 teleportPosition으로 이동시킨다</summar>
    public void Teleport(Vector3 teleportPosition)
    {
        if (CurrentView.Equals(E_ViewType.View3D))
            m_player3D_Object.transform.position = teleportPosition;
        else
            m_player2D_Object.transform.position = teleportPosition;
    }
}
