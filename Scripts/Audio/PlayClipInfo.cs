using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class PlayClipInfo
{
    [SerializeField]
    private string m_playName;
    /// <summary>재생 이름</summary>
    public string PlayName { get { return m_playName; } }

    /// <summary>클립들</summary>
    [SerializeField]
    private List<AudioClip> m_clips;

    [SerializeField]
    private bool m_isLoop;
    /// <summary>반복 여부</summary>
    public bool IsLoop { get { return m_isLoop; } }

    /// <summary>랜덤 여부</summary>
    [SerializeField]
    private bool m_isRandom;

    /// <summary>클립 개수</summary>
    private int m_clipCount = -1;

    /// <summary>이전 클립 인덱스</summary>
    private int m_previousClipIndex = -1;

    /// <summary>클립을 반환</summary>
    public AudioClip GetClip()
    {
        AudioClip clip = null;

        if (m_isRandom)
            clip = GetClip_Random();
        else
            clip = m_clips[0];

        return clip;
    }

    /// <summary>랜덤한 클립을 반환</summary>
    private AudioClip GetClip_Random()
    {
        // 클립 개수 초기화
        if (m_clipCount.Equals(-1))
            m_clipCount = m_clips.Count;

        AudioClip clip = null;

        int zero = 0;

        while(true)
        {
            int randomIndex = Random.Range(zero, m_clipCount);

            // 이전 인덱스랑 같지 않을경우 반복을 끝내고 클립 받아오기
            if(!randomIndex.Equals(m_previousClipIndex))
            {
                clip = m_clips[randomIndex];
                m_previousClipIndex = randomIndex;
                break;
            }
        }

        return clip;
    }
}
