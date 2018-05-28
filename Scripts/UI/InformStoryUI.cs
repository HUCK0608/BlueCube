using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class InformStoryUI : MonoBehaviour
{
    [Header("Can Change")]

    /// <summary>지속시간</summary>
    [SerializeField]
    private float m_durationTime;

    [Header("Don't Touch")]

    /// <summary>내용 텍스트</summary>
    [SerializeField]
    private Text m_contentsText;

    /// <summary>스토리 알림 코루틴</summary>
    private Coroutine m_informStoryCor;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    /// <summary>스토리 알림</summary>
    public void InformStory(string contents)
    {
        m_contentsText.text = contents;
        gameObject.SetActive(true);

        // 스토리 알림 코루틴 실행
        if (m_informStoryCor == null)
            m_informStoryCor = StartCoroutine(InformStoryLogic());
        else
        {
            StopCoroutine(m_informStoryCor);
            m_informStoryCor = StartCoroutine(InformStoryLogic());
        }
    }

    /// <summary>스토리 알림 로직</summary>
    private IEnumerator InformStoryLogic()
    {
        float addTime = 0f;

        while(true)
        {
            addTime += Time.deltaTime;

            if (addTime >= m_durationTime || UIManager.Instance.IsOnTabUI)
                break;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
