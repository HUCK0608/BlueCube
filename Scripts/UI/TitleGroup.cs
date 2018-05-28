using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class TitleGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject m_selectTitleImage;

    [SerializeField]
    private Text m_titleText;
    public string TitleName { get { return m_titleText.text; } set { m_titleText.text = value; } }

    private bool m_isLock = true;
    /// <summary>스토리가 잠겨있을 경우 true를 반환</summary>
    public bool IsLock { get { return m_isLock; } }

    private void Awake()
    {
        AddPointerEnterEvent();
        AddPointerClickEvent();
    }

    private void AddPointerEnterEvent()
    {
        EventTrigger trigger = GetComponentInChildren<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => UIManager.Instance.GetComponent<AudioPlayer>().Play("MouseEnter"));
        trigger.triggers.Add(entry);
    }

    /// <summary>마우스 클릭 이벤트 추가</summary>
    private void AddPointerClickEvent()
    {
        EventTrigger trigger = GetComponentInChildren<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => UIManager.Instance.StoryUI.SelectStory(this));
        entry.callback.AddListener((data) => UIManager.Instance.GetComponent<AudioPlayer>().Play("MouseClick"));
        trigger.triggers.Add(entry);
    }

    /// <summary>잠금 해제</summary>
    public void UnLock(string titleName)
    {
        m_titleText.text = titleName;
        m_isLock = false;
    }

    /// <summary>선택 이미지 활성화 설정</summary>
    public void SetSelectImageEnable(bool value)
    {
        m_selectTitleImage.SetActive(value);
    }

    /// <summary>텍스트 색상 설정</summary>
    public void SetTextColor(Color color)
    {
        m_titleText.color = color;
    }
}
