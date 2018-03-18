using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStat))]
[RequireComponent(typeof(PlayerController))]
public sealed class PlayerManager : MonoBehaviour
{
    private static PlayerManager m_instance;
    public static PlayerManager Instance { get { return m_instance; } }

    // 반환 필드
    /// <summary>시점변환 중이면 true를 반환</summary>
    public bool isViewChanging { get { return m_controller.Skill_CV.IsChanging; } }
    /// <summary>현재 시점을 반환</summary>
    public E_ViewType CurrentView { get { return m_controller.Skill_CV.ViewType; } }

    // 플레이어관련 스크립트 필드
    private PlayerController m_controller;
    /// <summary>컨트롤러</summary>
    public PlayerController Controller { get { return m_controller; } }

    private PlayerStat m_stat;
    /// <summary>스탯</summary>
    public PlayerStat Stat { get { return m_stat; } }

    private PlayerInventory m_inventory;
    /// <summary>인벤토리</summary>
    public PlayerInventory Inventory { get { return m_inventory; } }

    // 오브젝트 필드
    private GameObject m_player2D_GameObject;
    private GameObject m_player3D_GameObject;
    /// <summary>2D플레이어 게임오브젝트</summary>
    public GameObject Player2D_GameObject { get { return m_player2D_GameObject; } }
    /// <summary>3D플레이어 게임오브젝트</summary>
    public GameObject Player3D_GameObject { get { return m_player3D_GameObject; } }
    
    private void Awake()
    {
        m_instance = GetComponent<PlayerManager>();

        m_controller = GetComponent<PlayerController>();
        m_stat = GetComponent<PlayerStat>();
        m_inventory = GetComponent<PlayerInventory>();

        m_player2D_GameObject = transform.Find("Player2D").gameObject;
        m_player3D_GameObject = transform.Find("Player3D").gameObject;
    }

    /// <summary>플레이어 변경(currentView = 현재시점)</summary>
    public void ChangePlayer(E_ViewType currentView)
    {
        // 현재시점이 2D인 경우
        if(currentView.Equals(GameLibrary.Enum_View2D))
        {
            // 3D캐릭터 비활성화
            m_player3D_GameObject.SetActive(false);
            // 2D캐릭터의 부모를 그룹의 루트로 변경
            m_player2D_GameObject.transform.parent = transform;
            // 2D캐릭터의 각도를 0도로 만듬
            m_player2D_GameObject.transform.eulerAngles = Vector3.zero;
            // 3D캐릭터의 부모를 2D캐릭터로 변경
            m_player3D_GameObject.transform.parent = m_player2D_GameObject.transform;
            // 2D캐릭터 활성화
            m_player2D_GameObject.SetActive(true);
        }
        // 현재시점이 3D인 경우
        else
        {
            // 2D캐릭터 비활성화
            m_player2D_GameObject.SetActive(false);
            // 3D캐릭터의 부모를 그룹의 루트로 변경
            m_player3D_GameObject.transform.parent = transform;
            // 2D캐릭터의 부모를 3D캐릭터로 변경
            m_player2D_GameObject.transform.parent = m_player3D_GameObject.transform;
            // 3D캐릭터 활성화
            m_player3D_GameObject.SetActive(true);
        }
    }
}
