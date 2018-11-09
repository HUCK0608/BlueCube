using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class StoryUI : MonoBehaviour
{
    [Header("Can Change")]
    /// <summary>모든 스토리 개수</summary>
    [SerializeField]
    private int m_allStoryCount;
    /// <summary>글자 기본 색상</summary>
    [SerializeField]
    private Color m_textDefaultColor;
    /// <summary>글자 선택 색상</summary>
    [SerializeField]
    private Color m_textSelectColor;
    /// <summary>글자 그리기 딜레이</summary>
    [SerializeField]
    private float m_drawLetterDelay;
    /// <summary>스토리 리스트 스크롤 민감도</summary>
    [SerializeField]
    private float m_storyListScrollSensitivity;

    [Header("Don't touch!")]
    /// <summary>타이틀 그룹 프리팹</summary>
    [SerializeField]
    private GameObject m_storyTitleGroupPrefab;
    /// <summary>타이틀 리스트 콘텐츠</summary>
    [SerializeField]
    private RectTransform m_storyTitleListContents;
    /// <summary>컨텐츠 텍스트</summary>
    [SerializeField]
    private Text m_contentsText;
    /// <summary>스토리 알림 스크립트</summary>
    [SerializeField]
    private InformStoryUI m_informStoryUI;
    /// <summary>스토리 알림 스크립트</summary>
    public InformStoryUI InformStoryUI { get { return m_informStoryUI; } }

    /// <summary>타이틀 그룹모음</summary>
    private List<TitleGroup> m_titleGroups;
    /// <summary>언락한 스토리 모음</summary>
    private Dictionary<int, Item_Story> m_unLockStories;
    /// <summary>언락한 스토리 개수</summary>
    private int m_unLockStoryCount;
    public int UnLockStoryCount { get { return m_unLockStoryCount; } }
    /// <summary>현재 선택한 스토리 인덱스</summary>
    private int m_selectStoryIndex = -1;

    /// <summary>한 글자씩 그려주는 코루틴 변수</summary>
    private Coroutine m_drawOneLetterCor;
    /// <summary>Axis에서 마우스 휠 값을 불러오기 위한 변수</summary>
    private string m_mouseWheelString = "Mouse ScrollWheel";

    private void Awake()
    {
        m_titleGroups = new List<TitleGroup>();
        m_unLockStories = new Dictionary<int, Item_Story>();

        CreateTitleList();
    }

    /// <summary>타이틀 리스트 생성</summary>
    private void CreateTitleList()
    {
        for (int i = 0; i < m_allStoryCount; i++)
        {
            GameObject newTitleGroupObject = Instantiate(m_storyTitleGroupPrefab);
            newTitleGroupObject.transform.SetParent(m_storyTitleListContents);

            TitleGroup newTitleGroup = newTitleGroupObject.GetComponent<TitleGroup>();
            newTitleGroup.SetTextColor(m_textDefaultColor);

            // 리스트에 추가
            m_titleGroups.Add(newTitleGroup);
        }

        Vector2 newContentsBoxSize = m_storyTitleListContents.sizeDelta;
        newContentsBoxSize.y *= m_allStoryCount;

        m_storyTitleListContents.sizeDelta = newContentsBoxSize;
    }

    private void OnEnable()
    {
        // 글자 처음부터 다시그리기
        if (!m_unLockStoryCount.Equals(0))
            DrawContentsText();
    }

    //private void Update()
    //{
    //    SetStoryListScroll();
    //}

    ///// <summary>스토리 리스트 스크롤</summary>
    //private void SetStoryListScroll()
    //{
    //    if (Input.GetAxis(m_mouseWheelString) > 0)
    //        m_storyListScrollBar.value = Mathf.Clamp(m_storyListScrollBar.value + m_storyListScrollSensitivity * Time.deltaTime, 0, 1);
    //    else if (Input.GetAxis(m_mouseWheelString) < 0)
    //        m_storyListScrollBar.value = Mathf.Clamp(m_storyListScrollBar.value - m_storyListScrollSensitivity * Time.deltaTime, 0, 1);
    //}

    /// <summary>스토리 언락</summary>
    public void UnlcokStory(Item_Story story)
    {
        // 스토리 알림
        m_informStoryUI.InformStory(story.Contents);

        m_unLockStories.Add(story.StoryNumber, story);
        m_unLockStoryCount++;

        TitleGroup m_titleGroup = m_titleGroups[story.StoryNumber];
        m_titleGroup.UnLock(story.TitleName);

        // 현재 선택된 스토리가 없다면 선택 상태로 설정
        if (m_selectStoryIndex.Equals(-1))
        {
            m_selectStoryIndex = story.StoryNumber;

            m_titleGroup.SetTextColor(m_textSelectColor);
            m_titleGroup.SetSelectImageEnable(true);
        }
    }

    /// <summary>스토리 선택</summary>
    public void SelectStory(TitleGroup titleGroup)
    {
        // 언락된 스토리가 하나도 없을 경우 리턴
        if (m_unLockStories.Equals(0))
            return;

        int titleGroupIndex = m_titleGroups.IndexOf(titleGroup);

        // 선택된 스토리와 언락된 스토리가 같을 경우 리턴
        // 잠겨있을 경우 리턴
        if (titleGroupIndex.Equals(m_selectStoryIndex) || titleGroup.IsLock)
            return;

        // 기존 선택된 스토리의 글자색을 바꾸고 이미지를 끔
        m_titleGroups[m_selectStoryIndex].SetSelectImageEnable(false);
        m_titleGroups[m_selectStoryIndex].SetTextColor(m_textDefaultColor);

        // 새롭게 선택된 스토리를 설정 후 글자색을 바꾸고 이미지를 킴
        m_selectStoryIndex = titleGroupIndex;
        m_titleGroups[m_selectStoryIndex].SetSelectImageEnable(true);
        m_titleGroups[m_selectStoryIndex].SetTextColor(m_textSelectColor);

        DrawContentsText();
    }

    /// <summary>스토리 내용을 그림</summary>
    private void DrawContentsText()
    {
        if(m_drawOneLetterCor != null)
            StopCoroutine(m_drawOneLetterCor);

        m_drawOneLetterCor = StartCoroutine(DrawOneLetter());
    }

    /// <summary>한 글자씩 그려주는 코루틴</summary>
    private IEnumerator DrawOneLetter()
    {
        // 내용 및 내용 길이
        string contents = m_unLockStories[m_selectStoryIndex].Contents;
        int letterCount = contents.Length;

        int zero = 0;
        int one = 1;
        int currentLetterCount = 1;

        WaitForSeconds drawLetterDelayYield = new WaitForSeconds(m_drawLetterDelay);

        while(true)
        {
            m_contentsText.text = contents.Substring(zero, currentLetterCount);
            currentLetterCount++;

            if ((currentLetterCount - one).Equals(letterCount))
                break;

            yield return drawLetterDelayYield;
        }
    }
}
