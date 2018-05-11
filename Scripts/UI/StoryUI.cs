using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryUI : MonoBehaviour
{
    // 스토리 구조체
    public struct Story
    {
        // 생성자
        public Story(string titleName, string contents) { m_titleName = titleName; m_contents = contents; }

        // 타이틀 명, 내용
        private string m_titleName;
        private string m_contents;

        // GET SET
        public string TitleName { get { return m_titleName; } set { m_titleName = value; } }
        public string Contents { get { return m_contents; } set { m_contents = value; } }
    }

    private static StoryUI m_instance;
    public static StoryUI Instance { get { return m_instance; } }

    /// <summary>스토리 모음</summary>
    private List<Story> m_stories;
    /// <summary>스토리 개수</summary>
    private int m_storyCount;

    private void Awake()
    {
        m_instance = this;

        m_stories = new List<Story>();
    }

    /// <summary>스토리 추가</summary>
    public void AddStory(string titleName, string contents)
    {
        Story newStory = new Story(titleName, contents);

        Debug.Log("Get New Story");
        Debug.Log("Title Name : " + newStory.TitleName);
        Debug.Log("Contents : " + newStory.Contents);

        m_stories.Add(newStory);
        m_storyCount++;
    }
}
