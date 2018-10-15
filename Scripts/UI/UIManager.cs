using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class UIManager : MonoBehaviour
{
    private static UIManager m_instance;
    public static UIManager Instance { get { return m_instance; } }

    [Header("Can Change")]
    /// <summary>TabUI 활성화 키</summary>
    [SerializeField]
    private KeyCode m_TabUIEnableKey;
    [SerializeField]
    private KeyCode m_returnTitleKey;

    [Header("Don't Touch")]
    [SerializeField]
    private GameObject m_tabUI;

    [SerializeField]
    private GameObject m_pauseUI;

    [SerializeField]
    private PlayerHpUI m_playerHpUI;
    /// <summary>플레이어 체력 UI 스크립트</summary>
    public PlayerHpUI PlayerHpUI { get { return m_playerHpUI; } }

    [SerializeField]
    private StoryUI m_storyUI;
    /// <summary>스토리 UI 스크립트</summary>
    public StoryUI StoryUI { get { return m_storyUI; } }

    [SerializeField]
    private DeadUI m_deadUI;
    ///<summary>죽음 UI 스크립트</summary>
    public DeadUI DeadUI { get { return m_deadUI; } }

    private string m_titleScenePath = "Title";

    /// <summary>Tab UI가 켜졌을 경우 true를 반환</summary>
    public bool IsOnTabUI { get { return m_tabUI.activeSelf; } }

    /// <summary>Pause UI가 켜졌을 경우 true를 반환</summary>
    public bool IsOnPauseUI { get { return m_pauseUI.activeSelf; } }

    /// <summary>Dead UI가 켜졌을 경우 true를 반환</summary>
    public bool IsOnDeadUI { get { return m_deadUI.ActiveSelf; } }

    private void Awake()
    {
        m_instance = this;
    }

    private void Start()
    {
        InitUI();
    }

    /// <summary>UI 초기화</summary>
    private void InitUI()
    {
        m_playerHpUI.SetPlayerHpText(PlayerManager.Instance.Stat.Hp);

        m_tabUI.SetActive(false);
        m_pauseUI.SetActive(false);
        m_deadUI.SetActive(false);
    }

    private void Update()
    {
        SetPauseUIEnable();
        SetTabUIEnable();
    }

    /// <summary>TabUI 활성화 설정</summary>
    private void SetTabUIEnable()
    {
        if (Input.GetKeyDown(m_TabUIEnableKey) && !m_deadUI.ActiveSelf && !m_pauseUI.activeSelf)
        {
            m_tabUI.SetActive(!m_tabUI.activeSelf);

            if (m_tabUI.activeSelf)
                GameManager.Instance.SetCursorEnable(true);
            else if (!m_tabUI.activeSelf && PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D))
                GameManager.Instance.SetCursorEnable(false);
        }
    }

    /// <summary>PauseUI 활성화 설정</summary>
    private void SetPauseUIEnable()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_deadUI.ActiveSelf && !m_tabUI.activeSelf)
            m_pauseUI.SetActive(!m_pauseUI.activeSelf);
    }

    /// <summary>타이틀 화면으로 돌아감</summary>
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(m_titleScenePath);
    }
}
