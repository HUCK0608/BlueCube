using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_HintObject : MonoBehaviour
{
    // 힌트 모음
    private List<GameObject> m_hints;

    // 힌트 개수
    private int m_hintCount;

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
        for (int i = 0; i < m_hintCount; i++)
            m_hints[i].SetActive(true);
    }
}
