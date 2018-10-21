using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private Image m_acceptButtonImage, m_cancelButtonImage;

    [SerializeField]
    private Sprite m_acceptDefaultSprite, m_acceptSelectSprite;

    [SerializeField]
    private Sprite m_cancelDefaultSprite, m_cancelSelectSprite;

    private void OnEnable()
    {
        InitSprite();

        GameManager.Instance.SetCursorEnable(true);
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D))
            GameManager.Instance.SetCursorEnable(false);
    }

    private void InitSprite()
    {
        m_acceptButtonImage.sprite = m_acceptDefaultSprite;
        m_cancelButtonImage.sprite = m_cancelDefaultSprite;
    }

    public void AcceptPointerEnterEvent()
    {
        m_acceptButtonImage.sprite = m_acceptSelectSprite;
    }

    public void AcceptPointerExitEvent()
    {
        m_acceptButtonImage.sprite = m_acceptDefaultSprite;
    }

    public void AcceptPointerClickEvent()
    {
        UIManager.Instance.ReturnToTitle();
    }

    public void CancelPointerEnterEvent()
    {
        m_cancelButtonImage.sprite = m_cancelSelectSprite;
    }

    public void CancelPointerExitEvent()
    {
        m_cancelButtonImage.sprite = m_cancelDefaultSprite;
    }

    public void CancelPointerClickEvent()
    {
        gameObject.SetActive(false);
    }
}
