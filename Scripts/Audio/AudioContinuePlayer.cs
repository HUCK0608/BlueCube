using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioContinuePlayer : MonoBehaviour
{
    [System.Serializable]
    private struct ClipInfo
    {
        [SerializeField]
        private AudioClip m_audioClip;
        /// <summary>오디오 클립</summary>
        public AudioClip AudioClip { get { return m_audioClip; } }

        [SerializeField]
        private bool m_Loop;
        /// <summary>반복 여부</summary>
        public bool Loop { get { return m_Loop; } }
    }

    /// <summary>오디오 소스</summary>
    [SerializeField]
    private AudioSource m_audioSource;

    /// <summary>연속 재생 클립 정보</summary>
    [SerializeField]
    private ClipInfo[] m_continueClip;

    /// <summary>연속 재생 클립 개수</summary>
    private int m_continueClipCount;

    private void Awake()
    {
        m_continueClipCount = m_continueClip.Length;

        StartCoroutine(PlayLogic());
    }

    /// <summary>재생 로직</summary>
    private IEnumerator PlayLogic()
    {
        m_audioSource.loop = m_continueClip[0].Loop;
        m_audioSource.clip = m_continueClip[0].AudioClip;
        m_audioSource.Play();

        for(int i = 1; i < m_continueClipCount; i++)
        {
            yield return new WaitUntil(() => !m_audioSource.isPlaying);
            m_audioSource.loop = m_continueClip[i].Loop;
            m_audioSource.clip = m_continueClip[i].AudioClip;
            m_audioSource.Play();
        }
    }
}
