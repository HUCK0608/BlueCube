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

    EventTrigger trigger;

    private void Awake()
    {
        trigger = GetComponentInChildren<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => Test((PointerEventData)data));
        trigger.triggers.Add(entry);
    }

    public void Test(PointerEventData data)
    {
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

    public void OnPointerClick(PointerEventData dd)
    {
        Debug.Log("call");
    }
}
