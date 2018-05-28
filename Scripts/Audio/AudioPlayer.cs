using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioPlayer : MonoBehaviour
{
    /// <summary>오디오 소스</summary>
    [SerializeField]
    private AudioSource m_audioSource;

    /// <summary>플레이 클립 정보</summary>
    [SerializeField]
    private PlayClipInfo[] m_playClipInfo;

    /// <summary>플레이 클립 딕셔너리</summary>
    private Dictionary<string, PlayClipInfo> m_playClipInfoDictionary;

    private void Awake()
    {
        InitPlayClipInfoDictionary();
    }

    /// <summary>딕셔너리 초기화</summary>
    private void InitPlayClipInfoDictionary()
    {
        m_playClipInfoDictionary = new Dictionary<string, PlayClipInfo>();

        int m_playClipInfoCount = m_playClipInfo.Length;

        for(int i = 0; i < m_playClipInfoCount; i++)
            m_playClipInfoDictionary.Add(m_playClipInfo[i].PlayName, m_playClipInfo[i]);
    }

    /// <summary>오디오 재생</summary>
    public void Play(string playName)
    {
        PlayClipInfo playClipInfo = m_playClipInfoDictionary[playName];

        if (!m_audioSource.loop.Equals(playClipInfo.IsLoop))
            m_audioSource.loop = playClipInfo.IsLoop;

        AudioClip playClip = m_playClipInfoDictionary[playName].GetClip();

        m_audioSource.clip = playClip;

        m_audioSource.PlayOneShot(playClip);
    }
}
