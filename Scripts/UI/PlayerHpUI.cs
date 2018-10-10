using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerHpUI : MonoBehaviour
{
    [Header("Can Change")]
    /// <summary>텍스트 기본 색상</summary>
    [SerializeField]
    private Color m_textDefaultColor;
    /// <summary>텍스트 변경 색상</summary>
    [SerializeField]
    private Color m_textChangeColor;
    /// <summary>아웃라인 변경 색상</summary>
    [SerializeField]
    private float m_textChangeColorDurationTime;

    [Header("Don't Touch")]
    /// <summary>hp 텍스트</summary>
    [SerializeField]
    private Text m_playerHpText;

    /// <summary>일정시간 색상변경 코루틴 변수</summary>
    private Coroutine m_playerHpTextColorChangeCor;

    private Outline[] m_outlines;
    private int m_outlineCount;

    private void Awake()
    {
        m_outlines = m_playerHpText.GetComponents<Outline>();
        m_outlineCount = m_outlines.Length;
    }

    /// <summary>플레이어 체력 텍스트를 변경</summary>
    public void SetPlayerHpText(int hp)
    {
        m_playerHpText.text = hp.ToString();

        if (m_playerHpTextColorChangeCor != null)
            StopCoroutine(m_playerHpTextColorChangeCor);
        m_playerHpTextColorChangeCor = StartCoroutine(PlayerHpTextColorChange());
    }

    /// <summary>플레이어 체력 텍스트의 색을 일정시간 변경</summary>
    private IEnumerator PlayerHpTextColorChange()
    {
        m_playerHpText.color = m_textChangeColor;
        SetOutlineEnable(true);

        float addTime = 0f;

        while(true)
        {
            addTime += Time.deltaTime;

            if (addTime >= m_textChangeColorDurationTime)
                break;

            yield return null;
        }

        m_playerHpText.color = m_textDefaultColor;
        SetOutlineEnable(false);

        m_playerHpTextColorChangeCor = null;
    }

    /// <summary>아웃라인 활성화 설정</summary>
    private void SetOutlineEnable(bool value)
    {
        for (int i = 0; i < m_outlineCount; i++)
            m_outlines[i].enabled = value;
    }
}
