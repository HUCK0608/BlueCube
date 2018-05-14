using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class StoryManager : MonoBehaviour
{
    private static StoryManager m_instance;
    public static StoryManager Instance { get { return m_instance; } }

    /// <summary>모든 스토리 개수</summary>
    [SerializeField]
    private int m_allStoryCount;

    [SerializeField]
    private Color m_defaultTextColor;
    [SerializeField]
    private Color m_selectTextColor;

    [Header("Don't touch!")]
    /// <summary>타이틀 그룹 프리팹</summary>
    [SerializeField]
    private GameObject m_titleGroupPrefab;
    /// <summary>타이틀 그룹모음</summary>
    private List<TitleGroup> m_titleGroups;

    /// <summary>타이틀 리스트 콘텐츠</summary>
    [SerializeField]
    private RectTransform m_titleListContents;

    /// <summary>습득한 스토리 모음</summary>
    private Dictionary<int, Item_Story> m_haveStories;
    /// <summary>습득한 스토리 개수</summary>
    private int m_haveStoryCount;

    /// <summary>현재 선택한 스토리 인덱스</summary>
    private int m_selectStoryIndex = -1;

    [SerializeField]
    private Text m_contentsText;

    private void Awake()
    {
        m_instance = this;

        m_titleGroups = new List<TitleGroup>();
        m_haveStories = new Dictionary<int, Item_Story>();

        CreateTitleList();
    }

    private void OnEnable()
    {
        if(!m_selectStoryIndex.Equals(-1))
            m_contentsText.text = m_haveStories[m_selectStoryIndex].Contents;
    }

    /// <summary>타이틀 리스트 생성</summary>
    private void CreateTitleList()
    {
        for(int i = 0; i < m_allStoryCount; i++)
        {
            GameObject newTitleGroupObject = Instantiate(m_titleGroupPrefab);
            newTitleGroupObject.transform.SetParent(m_titleListContents);

            TitleGroup newTitleGroup = newTitleGroupObject.GetComponent<TitleGroup>();
            newTitleGroup.SetTextColor(m_defaultTextColor);

            // 리스트에 추가
            m_titleGroups.Add(newTitleGroup);
        }

        Vector2 newContentsBoxSize = m_titleListContents.sizeDelta;
        newContentsBoxSize.y *= m_allStoryCount;

        m_titleListContents.sizeDelta = newContentsBoxSize;
    }

    /// <summary>스토리 추가</summary>
    public void AddStory(Item_Story story)
    {
        m_haveStories.Add(story.StoryNumber, story);
        m_haveStoryCount++;

        TitleGroup m_titleGroup = m_titleGroups[story.StoryNumber];
        m_titleGroup.UnLock(story.TitleName);

        // 현재 선택된 스토리가 없다면 선택 상태로 설정
        if (m_selectStoryIndex.Equals(-1))
        {
            m_selectStoryIndex = story.StoryNumber;

            m_titleGroup.SetTextColor(m_selectTextColor);
            m_titleGroup.SetSelectImageEnable(true);
        }
    }

    /// <summary>스토리 선택</summary>
    public void SelectStory(TitleGroup titleGroup)
    {
        
    }
}
