using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_HintObject : MonoBehaviour
{
    // 힌트 모음
    private List<GameObject> m_hints;

    // 힌트 개수
    private int m_hintCount;

    // 힌트 활성화 시 딜레이 시간
    [SerializeField]
    private float m_hintEnableDelayTime;

    // 힌트가 활성화 되었는지 여부
    private bool m_isOn;

    private void Awake()
    {
        // 힌트 초기화
        m_hints = new List<GameObject>();

        Transform hints = transform.Find("Hints");

        m_hintCount = hints.childCount;

        for(int i = 0; i < m_hintCount; i++)
        {
            m_hints.Add(hints.GetChild(i).gameObject);
            m_hints[i].SetActive(false);
        }
    }

    /// <summary>힌트를 보여줌</summary>
    public void ShowHint()
    {
        // 이미 켜진 경우 리턴
        if (m_isOn)
            return;

        // 힌트를 순차적으로 보여줌
        StartCoroutine(OnHintEnable());

        m_isOn = true;
    }

    // 힌트를 순차적으로 보여줌
    private IEnumerator OnHintEnable()
    {
        for(int i = 0; i < m_hintCount; i++)
        {
            m_hints[i].SetActive(true);
            yield return new WaitForSeconds(m_hintEnableDelayTime);
        }
    }
}
