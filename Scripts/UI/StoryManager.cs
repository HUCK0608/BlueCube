using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    private static StoryManager m_instance;
    public static StoryManager Instance { get { return m_instance; } }

    /// <summary>스토리 모음</summary>
    private List<Item_Story> m_stories;
    /// <summary>스토리 개수</summary>
    private int m_storyCount;

    /// <summary>현재 선택한 스토리</summary>
    private int m_currentSelect = -1;

    [SerializeField]
    private Text m_selectTitleText;

    private void Awake()
    {
        m_instance = this;

        m_stories = new List<Item_Story>();
    }

    /// <summary>스토리 추가</summary>
    public void AddStory(Item_Story story)
    {
        m_stories.Add(story);
        m_storyCount++;

        // 스토리를 처음 얻는 것이면 선택상태로 만듬
        if(m_currentSelect.Equals(-1))
        {
            m_currentSelect = 0;
            m_selectTitleText.text = story.TitleName;
        }
    }

    /// <summary>스토리 선택</summary>
    private void SelectStory()
    {

    }
}
