using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class Title_SelectMenuUI : MonoBehaviour
{
    /// <summary>기본 텍스트 색상</summary>
    [SerializeField]
    private Color m_textDefaultColor;

    /// <summary>선택 텍스트 색상</summary>
    [SerializeField]
    private Color m_textSelectColor;

    /// <summary>뉴 게임 씬</summary>
    [SerializeField]
    private string m_newGameScenePath;

    /// <summary>선택 메뉴 텍스트 컴포넌트 모음</summary>
    private List<Text> m_selectMenuTexts;

    private void Awake()
    {
        m_selectMenuTexts = new List<Text>();
        m_selectMenuTexts.AddRange(GetComponentsInChildren<Text>());
    }

    /// <summary>마우스가 메뉴에 들어올 때 이벤트</summary>
    public void OnMouseEnterEvent(Text menuText)
    {
        menuText.color = m_textSelectColor;
    }

    /// <summary>마우스가 메뉴에서 나갈 때 이벤트</summary>
    public void OnMouseExitEvent(Text menuText)
    {
        menuText.color = m_textDefaultColor;
    }

    /// <summary>새 게임 클릭 이벤트</summary>
    public void OnClickNewGameMenuEvent()
    {
        SceneManager.LoadScene(m_newGameScenePath);
    }
}
