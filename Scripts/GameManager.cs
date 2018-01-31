﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    // 인스턴스
    private static GameManager m_instance;
    public static GameManager Instance { get { return m_instance; } }

    // 3D 카메라 매니저
    private CameraManager m_cameraManager;
    public CameraManager CameraManager { get { return m_cameraManager; } }

    // 아이템 매니저
    private ItemManager m_itemManager;
    public ItemManager ItemManager { get { return m_itemManager; } }

    // 오브젝트 매니저
    private ObjectManager m_objectManager;
    public ObjectManager ObjectManager { get { return m_objectManager; } }

    // 플레이어 매니저
    private PlayerManager m_playerManager;
    public PlayerManager PlayerManager { get { return m_playerManager; } }

    // 블루큐브 매니저
    private BlueCubeManager m_blueCubeManager;
    public BlueCubeManager BlueCubeManager { get { return m_blueCubeManager; } }

    // 적 매니저
    private EnemyManager m_enemyManager;
    public EnemyManager EnemyManager { get { return m_enemyManager; } }

    // 이펙트 매니저
    private EffectManager m_effectManager;
    public EffectManager EffectManager { get { return m_effectManager; } }

    // UI 매니저
    private UIManager m_uiManager;
    public UIManager UIManager { get { return m_uiManager; } }

    // 현재시점
    private E_ViewType m_currentViewType;
    public E_ViewType ViewType { get { return m_currentViewType; } }

    private void Awake()
    {
        DisableCursor();

        m_instance = GetComponent<GameManager>();

        m_cameraManager = GameObject.Find("CameraGroup").GetComponent<CameraManager>();
        m_itemManager = GameObject.Find("Items").GetComponent<ItemManager>();
        m_objectManager = GameObject.Find("Objects").GetComponent<ObjectManager>();
        m_playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        m_blueCubeManager = GameObject.Find("BlueCube").GetComponent<BlueCubeManager>();
        m_enemyManager = GameObject.Find("Enemies").GetComponent<EnemyManager>();
        m_effectManager = GameObject.Find("Effects").GetComponent<EffectManager>();
        m_uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    // 커서표시 잠금
    private void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // 뷰 변경
    public void ChangeViewType()
    {
        // 다음 시점을 구함
        int newViewType = ((int)m_currentViewType + 1) % 2;
        // 다음 시점을 저장
        m_currentViewType = (E_ViewType)newViewType;

        // 플레이어 변경
        m_playerManager.ChangePlayer();

        // 블루큐브 변경
        m_blueCubeManager.ChangeCube();

        if (m_currentViewType == E_ViewType.View3D)
        {
            // 오브젝트 변경
            m_objectManager.ChangeObjects();
            
            // 적 변경
            m_enemyManager.ChangeEnemies();
            // 아이템 변경
            m_itemManager.ChangeItems();
        }
    }
}
