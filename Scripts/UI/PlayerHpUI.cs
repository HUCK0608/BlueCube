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
    /// <summary>텍스트 색상 변경 지속시간</summary>
    [SerializeField]
    private float m_textChangeColorDurationTime;

    [Header("Don't Touch")]
    /// <summary>hp 텍스트</summary>
    [SerializeField]
    private Text m_playerHpText;

    /// <summary>일정시간 색상변경 코루틴 변수</summary>
    private Coroutine m_playerHpTextColorChangeCor;

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

        float addTime = 0f;

        while(true)
        {
            addTime += Time.deltaTime;

            if (addTime >= m_textChangeColorDurationTime)
                break;

            yield return null;
        }

        m_playerHpText.color = m_textDefaultColor;

        m_playerHpTextColorChangeCor = null;
    }
}
