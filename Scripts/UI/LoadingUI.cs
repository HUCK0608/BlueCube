using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LoadingUI : MonoBehaviour
{
    private static LoadingUI m_instance;
    public static LoadingUI Instance { get { return m_instance; } }

    private static string m_fadeInPath = "FadeIn";

    private Animator m_animator;

    /// <summary>게임을 시작할 때 fadeOut이 켜져있을지 여부</summary>
    [SerializeField]
    private bool m_isOnStartFadeOut;

    private void Awake()
    {
        m_instance = this;
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.SetActive(m_isOnStartFadeOut);
    }

    public void PlayFadeIn()
    {
        gameObject.SetActive(true);
        m_animator.Play(m_fadeInPath);
    }
}
