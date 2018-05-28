using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class DeadUI : MonoBehaviour
{
    [Header("Can Change")]
    /// <summary>게임오버 페이드 인 시간</summary>
    [SerializeField]
    private float m_gameOverFadeInTime;
    /// <summary>게임오버 지속시간</summary>
    [SerializeField]
    private float m_gameOverDurationTime;
    /// <summary>게임오버 페이드 아웃 시간</summary>
    [SerializeField]
    private float m_gameOverFadeOutTime;
    /// <summary>컨티뉴 페이드 인 시간</summary>
    [SerializeField]
    private float m_continueFadeInTime;

    [Header("Don't Touch")]
    [SerializeField]
    private Image m_gameOverImage;
    [SerializeField]
    private Image m_continueImage;

    [Space(10f)]

    [SerializeField]
    private Image m_titleImage;
    [SerializeField]
    private Sprite m_titleDefaultSprite;
    [SerializeField]
    private Sprite m_titleSelectSprite;

    [Space(10f)]

    [SerializeField]
    private Image m_retryImage;
    [SerializeField]
    private Sprite m_retryDefaultSprite;
    [SerializeField]
    private Sprite m_retrySelectSprite;

    private void Awake()
    {
    }

    /// <summary>활성화 설정</summary>
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);

        if (value)
            StartCoroutine(DeadUILogic());
    }

    /// <summary>활성화 여부를 반환</summary>
    public bool ActiveSelf { get { return gameObject.activeSelf; } }

    /// <summary>Dead UI 로직</summary>
    private IEnumerator DeadUILogic()
    {
        float zero = 0f;
        float one = 1f;

        float gameOverFadeInValue = one / m_gameOverFadeInTime;
        float gameOverDurationValue = one / m_gameOverDurationTime;
        float gameOverFadeOutValue = one / m_gameOverFadeOutTime;
        float continueFadeInTime = one / m_continueFadeInTime;

        // GameOver FadeIn
        while(true)
        {
            Color newColor = m_gameOverImage.color;
            newColor.a += gameOverFadeInValue * Time.deltaTime;
            newColor.a = Mathf.Clamp(newColor.a, zero, one);
            m_gameOverImage.color = newColor;

            if (m_gameOverImage.color.a.Equals(one))
                break;

            yield return null;
        }

        // GameOver Duration
        float addTime = 0f;

        while(true)
        {
            addTime += gameOverDurationValue * Time.deltaTime;

            if (addTime >= m_gameOverDurationTime)
                break;

            yield return null;
        }

        // GameOver FadeOut
        while(true)
        {
            Color newGameOverColor = m_gameOverImage.color;
            newGameOverColor.a -= gameOverFadeOutValue * Time.deltaTime;
            newGameOverColor.a = Mathf.Clamp(newGameOverColor.a, zero, one);
            m_gameOverImage.color = newGameOverColor;

            if (m_gameOverImage.color.a.Equals(zero))
                break;

            yield return null;
        }

        // Continue FadeIn
        while(true)
        {
            Color newContinueColor = m_continueImage.color;
            newContinueColor.a += continueFadeInTime * Time.deltaTime;
            newContinueColor.a = Mathf.Clamp(newContinueColor.a, zero, one);
            m_continueImage.color = newContinueColor;
            m_titleImage.color = newContinueColor;
            m_retryImage.color = newContinueColor;

            if (m_continueImage.color.a.Equals(one))
                break;

            yield return null;
        }
    }

    /// <summary>타이틀 MouseEnter 이벤트</summary>
    public void TitleOnMouseEnterEvent()
    {
        m_titleImage.sprite = m_titleSelectSprite;
    }

    /// <summary>타이틀 MouseExit 이벤트</summary>
    public void TitleOnMouseExitEvent()
    {
        m_titleImage.sprite = m_titleDefaultSprite;
    }

    /// <summary>타이틀 MouseClick 이벤트</summary>
    public void TitleOnMouseClickEvent()
    {
        UIManager.Instance.ReturnToTitle();
    }

    /// <summary>재시작 MouseEnter 이벤트</summary>
    public void RetryOnMouseEnterEvent()
    {
        m_retryImage.sprite = m_retrySelectSprite;
    }

    /// <summary>재시작 MouseExit 이벤트</summary>
    public void RetryOnMouseExitEvent()
    {
        m_retryImage.sprite = m_retryDefaultSprite;
    }

    /// <summary>재시작 MouseClick 이벤트</summary>
    public void RetryOnMouseCilckEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
