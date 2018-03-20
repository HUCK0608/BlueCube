using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    // 인스턴스
    private static GameManager m_instance;
    public static GameManager Instance { get { return m_instance; } }

    // 월드 매니저
    private WorldManager m_worldManager;
    public WorldManager WorldManager { get { return m_worldManager; } }

    // 3D 카메라 매니저
    private CameraManager m_cameraManager;
    public CameraManager CameraManager { get { return m_cameraManager; } }

    // 플레이어 매니저
    private PlayerManager_Old m_playerManager;
    public PlayerManager_Old PlayerManager { get { return m_playerManager; } }

    // 블루큐브 매니저
    private BlueCubeManager m_blueCubeManager;
    public BlueCubeManager BlueCubeManager { get { return m_blueCubeManager; } }

    // 총알 매니저
    private BulletManager m_bulletManager;
    public BulletManager BulletManager { get { return m_bulletManager; } }

    // 이펙트 매니저
    private EffectManager m_effectManager;
    public EffectManager EffectManager { get { return m_effectManager; } }

    private LightManager m_lightManager;
    public LightManager LightManager { get { return m_lightManager; } }

    private void Awake()
    {
        //DisableCursor();
        m_instance = GetComponent<GameManager>();

        m_worldManager = GameObject.Find("World").GetComponent<WorldManager>();
        m_cameraManager = GameObject.Find("CameraGroup").GetComponent<CameraManager>();
        m_playerManager = GameObject.Find("PlayerGroup").GetComponent<PlayerManager_Old>();
        m_blueCubeManager = GameObject.Find("BlueCube").GetComponent<BlueCubeManager>();
        m_bulletManager = GameObject.Find("Bullets").GetComponent<BulletManager>();
        m_effectManager = GameObject.Find("Effects").GetComponent<EffectManager>();
        m_lightManager = GameObject.Find("Light").GetComponent<LightManager>();
    }

    // 커서표시 잠금
    private void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
