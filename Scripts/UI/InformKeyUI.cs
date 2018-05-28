using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class InformKeyUI : MonoBehaviour
{
    [Header("Don't Touch")]

    /// <summary>키 텍스트</summary>
    [SerializeField]
    private Text m_keyText;

    /// <summary>Tab 키 텍스트</summary>
    [SerializeField]
    private Text m_tabKeyText;

    /// <summary>활성화 위치</summary>
    [SerializeField]
    private Vector2 m_enablePosition;

    /// <summary>비활성화 위치</summary>
    [SerializeField]
    private Vector2 m_disablePosition;

    [Header("Can Change")]

    /// <summary>이동속도</summary>
    [SerializeField]
    private float m_moveSpeed;

    /// <summary>지속 시간</summary>
    [SerializeField]
    private float m_durationTime;

    /// <summary>Rect Transform</summary>
    private RectTransform m_transform;

    /// <summary>알림이 활성화 되어있으면 true를 반환</summary>
    private bool m_isOnInform;

    /// <summary>새로운 알림이 있으면 true를 반환</summary>
    private bool m_isOnNewInform;

    private void Awake()
    {
        m_transform = this.GetComponent<RectTransform>();
    }

    /// <summary>키 획득을 알림</summary>
    public void InformKey(int currentKey)
    {
        string currentKeyString = currentKey.ToString();
        m_keyText.text = currentKeyString;
        m_tabKeyText.text = currentKeyString;

        if (!m_isOnInform)
            StartCoroutine(InformLogic());
        else
            m_isOnNewInform = true;
    }

    private IEnumerator InformLogic()
    {
        m_isOnInform = true;

        // 활성화 이동
        while(true)
        {
            m_transform.anchoredPosition = Vector3.MoveTowards(m_transform.anchoredPosition, m_enablePosition, m_moveSpeed * Time.deltaTime);

            if (m_transform.anchoredPosition.Equals(m_enablePosition))
                break;

            yield return null;
        }

        float zero = 0f;
        float addTime = 0f;

        // 지속
        while (true)
        {
            addTime += Time.deltaTime;

            // 새로운 알림이 있으면 누적시간을 초기화
            if (m_isOnNewInform)
            {
                addTime = zero;
                m_isOnNewInform = false;
            }

            if (addTime >= m_durationTime)
                break;

            yield return null;
        }

        // 비활성화 이동
        while(true)
        {
            m_transform.anchoredPosition = Vector3.MoveTowards(m_transform.anchoredPosition, m_disablePosition, m_moveSpeed * Time.deltaTime);

            // 새로운 알림이 있으면 코루틴 종료
            if (m_isOnNewInform)
                break;

            if (m_transform.anchoredPosition.Equals(m_disablePosition))
                break;

            yield return null;
        }

        // 새로운 알림이 있으면 코루틴 재시작
        if (m_isOnNewInform)
        {
            m_isOnNewInform = false;
            StartCoroutine(InformLogic());
        }
        else
        {
            m_isOnInform = false;
        }
    }
}