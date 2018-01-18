using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시점타입
public enum E_ViewType { View2D = 0, View3D }

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
    private ObjectManager m_staticObjectManager;
    public ObjectManager StaticObjectManager { get { return m_staticObjectManager; } }

    // 지형 매니저
    private TerrainManager m_terrainManager;
    public TerrainManager TerrainManager { get { return m_terrainManager; } }

    // 총알 매니저
    private BulletManager m_bulletManager;
    public BulletManager BulletManager { get { return m_bulletManager; } }

    // 플레이어 매니저
    private PlayerManager m_playerManager;
    public PlayerManager PlayerManager { get { return m_playerManager; } }

    // 블루큐브 매니저
    private BlueCubeManager m_blueCubeManager;

    // 적 매니저
    private EnemyManager m_enemyManager;
    public EnemyManager EnemyManager { get { return m_enemyManager; } }

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
        m_staticObjectManager = GameObject.Find("Objects").GetComponent<ObjectManager>();
        m_terrainManager = GameObject.Find("Terrains").GetComponent<TerrainManager>();
        m_bulletManager = GameObject.Find("Bullets").GetComponent<BulletManager>();
        m_playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        m_blueCubeManager = GameObject.Find("BlueCube").GetComponent<BlueCubeManager>();
        m_enemyManager = GameObject.Find("Enemies").GetComponent<EnemyManager>();
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

        // 카메라 스위칭
        m_cameraManager.ChangeCamera();
        // 정적 오브젝트 변경
        m_staticObjectManager.ChangeObjects();
        // 지형 변경
        m_terrainManager.ChangeTerrain();
        // 총알 변경
        m_bulletManager.ChangeBullets();
        // 플레이어 변경
        m_playerManager.ChangePlayer();
        // 블루큐브 변경
        m_blueCubeManager.ChangeCube();
        // 적 변경
        m_enemyManager.ChangeEnemies();
        // 아이템 변경
        m_itemManager.ChangeItems();
    }
}
