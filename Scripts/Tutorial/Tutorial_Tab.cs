using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tutorial_Tab : Tutorial
{
    /// <summary>UI가 사라지는 시간</summary>
    [SerializeField]
    private float m_disableTime;

    /// <summary>TabUI 이미지</summary>
    private Image m_tabUIImage;

    private void Awake()
    {
        m_tabUIImage = GetComponent<Image>();
        m_tabUIImage.enabled = false;
    }

    public override void StartTutorial()
    {
        StartCoroutine(OnEnableCheck());
    }

    /// <summary>유아이가 켜지기 위한 체크</summary>
    private IEnumerator OnEnableCheck()
    {
        // 스토리 알림 UI가 꺼질때까지 대기
        yield return new WaitUntil(() => !UIManager.Instance.StoryUI.InformStoryUI.IsOnUI);
        m_tabUIImage.enabled = true;

        StartCoroutine(EndCheck());
    }

    private IEnumerator EndCheck()
    {
        // TabUI가 켜질때까지 대기
        yield return new WaitUntil(() => UIManager.Instance.IsOnTabUI);
        EndTutorial();
    }

    protected override void EndTutorial()
    {
        gameObject.SetActive(false);
    }
}
