using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class Title_SelectMenuUI : MonoBehaviour
{
    [Header("Don't Touch")]
    [SerializeField]
    private Image m_newGameImage;
    [SerializeField]
    private Sprite m_newGameDefaultSprite;
    [SerializeField]
    private Sprite m_newGameSelectSprite;

    [Space(10f)]

    [SerializeField]
    private Image m_loadGameImage;
    [SerializeField]
    private Sprite m_loadGameDefaultSprite;
    [SerializeField]
    private Sprite m_loadGameSelectSprite;

    [Space(10f)]

    [SerializeField]
    private Image m_optionsImage;
    [SerializeField]
    private Sprite m_optionsDefaultSprite;
    [SerializeField]
    private Sprite m_optionsSelectSprite;

    [Space(10f)]

    [SerializeField]
    private Image m_exitImage;
    [SerializeField]
    private Sprite m_exitDefaultSprite;
    [SerializeField]
    private Sprite m_exitSelectSprite;

    [Space(10f)]

    [SerializeField]
    private RectTransform m_selectImage;

    [Space(10f)]

    /// <summary>뉴 게임 씬</summary>
    [SerializeField]
    private string m_newGameScenePath;

    bool m_isOnNewGameLoad;

    public void OnMouseEnterNewGame() { m_newGameImage.sprite = m_newGameSelectSprite; EnableSelectImage(m_newGameImage.rectTransform); }
    public void OnMouseExitNewGame() { m_newGameImage.sprite = m_newGameDefaultSprite; DisableSelectImage(); }
    public void OnMouseClickNewGame()
    {
        if (!m_isOnNewGameLoad)
        {
            m_isOnNewGameLoad = true;
            StartCoroutine(NewGameLogic());
        }
    }
    private IEnumerator NewGameLogic()
    {
        LoadingUI.Instance.PlayFadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(m_newGameScenePath);
    }

    public void OnMouseEnterLoadGame() { m_loadGameImage.sprite = m_loadGameSelectSprite; EnableSelectImage(m_loadGameImage.rectTransform); }
    public void OnMouseExitLoadGame() { m_loadGameImage.sprite = m_loadGameDefaultSprite; DisableSelectImage(); }

    public void OnMouseEnterOptions() { m_optionsImage.sprite = m_optionsSelectSprite; EnableSelectImage(m_optionsImage.rectTransform); }
    public void OnMouseExitOptions() { m_optionsImage.sprite = m_optionsDefaultSprite; DisableSelectImage(); }

    public void OnMouseEnterExit() { m_exitImage.sprite = m_exitSelectSprite; EnableSelectImage(m_exitImage.rectTransform); }
    public void OnMouseExitExit() { m_exitImage.sprite = m_exitDefaultSprite; DisableSelectImage(); }
    public void OnMouseClickExit() { Application.Quit(); }

    private void EnableSelectImage(RectTransform target)
    {
        Vector2 newPosition = target.anchoredPosition;
        newPosition.x = -target.sizeDelta.x * 0.5f - 30f;
        newPosition.y = (-target.sizeDelta.y + m_selectImage.sizeDelta.y) * 0.5f + target.anchoredPosition.y;
        m_selectImage.anchoredPosition = newPosition;

        m_selectImage.gameObject.SetActive(true);
    }

    private void DisableSelectImage()
    {
        m_selectImage.gameObject.SetActive(false);
    }
}
