using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tutorial_GoodLuck : Tutorial
{
    /// <summary>UI 유지 시간</summary>
    [SerializeField]
    private float m_UIKeepTime;

    /// <summary>UI가 사라지는 시간</summary>
    [SerializeField]
    private float m_UIDisableTime;

    /// <summary>프레임 UI 이미지</summary>
    private Image m_frameUIImage;

    /// <summary>행운을 비는 텍스트</summary>
    private Text m_goodLuckText;

    private void Awake()
    {
        m_frameUIImage = GetComponent<Image>();
        m_goodLuckText = GetComponentInChildren<Text>();

        gameObject.SetActive(false);    
    }

    public override void StartTutorial()
    {
        gameObject.SetActive(true);
        StartCoroutine(EndCheck());
    }

    /// <summary>튜토리얼 종료 체크</summary>
    private IEnumerator EndCheck()
    {
        yield return new WaitForSeconds(m_UIKeepTime);

        EndTutorial();
    }

    protected override void EndTutorial()
    {
        StartCoroutine(EndTutorialLogic());
    }

    /// <summary>튜토리얼이 종료되는 로직</summary>
    private IEnumerator EndTutorialLogic()
    {
        float disableValue = 1.0f / m_UIDisableTime;

        Color currentColor = Color.white;

        float zero = 0.0f;
        float one = 1.0f;

        while(true)
        {
            currentColor.a = Mathf.Clamp(currentColor.a - disableValue * Time.deltaTime, zero, one);

            m_frameUIImage.color = currentColor;
            m_goodLuckText.color = currentColor;

            if (currentColor.a.Equals(zero))
                break;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
