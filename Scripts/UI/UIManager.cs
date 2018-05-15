using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEditor;

public sealed class UIManager : MonoBehaviour
{
    private static UIManager m_instance;
    public static UIManager Instance { get { return m_instance; } }

    private bool m_isOnUI;
    /// <summary>UI가 켜졌을 경우 true를 반환</summary>
    public bool IsOnUI { get { return m_isOnUI; } }

    [Header("Can Change")]
    /// <summary>UI 활성화 키</summary>
    [SerializeField]
    private KeyCode m_UIEnableKey;

    [Header("Don't Touch")]
    public PlayerHpUI m_playerHpUI;
    /// <summary>플레이어 체력 UI 스크립트</summary>
    public PlayerHpUI PlayerHpUI { get { return m_playerHpUI; } }

    [SerializeField]
    private StoryUI m_storyUI;
    /// <summary>스토리 UI 스크립트</summary>
    public StoryUI StoryUI { get { return m_storyUI; } }

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

        m_storyUI.SetEnabled(m_isOnUI);
    }

    private void Update()
    {
        SetStoryUIEnable();
        m_storyUI.SetStoryListScroll();
    }

    /// <summary>스토리UI 활성화 설정</summary>
    private void SetStoryUIEnable()
    {
        if (Input.GetKeyDown(m_UIEnableKey))
        {
            m_isOnUI = !m_isOnUI;
            m_storyUI.SetEnabled(m_isOnUI);
        }
    }
}
