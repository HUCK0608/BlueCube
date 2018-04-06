using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Destroy_FootStep : MonoBehaviour
{
    // 유지시간
    [SerializeField]
    private float m_keepTime;

    // 리젠시간
    [SerializeField]
    private float m_regenTime;

    // 부셔지는 속도
    //[SerializeField]
    //private float m_destroySpeed;

    private float m_addTime;

    private bool m_isBroken;

    private GameObject m_footStep2D;
    private GameObject m_footStep3D;

    private void Awake()
    {
        m_footStep2D = transform.Find("Collider2D").gameObject;
        m_footStep3D = transform.Find("ModelAndCollider3D").gameObject;
    }

    public void TimerOn()
    {
        if (m_isBroken)
            return;

        m_isBroken = true;

        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        m_addTime = 0f;

        while(true)
        {
            // 시점변환중이 아니고 탐지모드가 아니고 2D가 아닐경우 실행
            if (!GameLibrary.Bool_IsGameStop_Old)
            {
                m_addTime += Time.deltaTime;

                if (m_addTime >= m_keepTime)
                {
                    m_footStep2D.SetActive(false);
                    m_footStep3D.SetActive(false);
                    StartCoroutine(RegenTimer());
                    break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator RegenTimer()
    {
        m_addTime = 0f;

        while(true)
        {
            // 시점변환중이 아니고 탐지모드가 아니고 2D가 아닐경우 실행
            if (!GameLibrary.Bool_IsGameStop_Old)
            {
                m_addTime += Time.deltaTime;

                if (m_addTime >= m_regenTime)
                {
                    m_footStep2D.SetActive(true);
                    m_footStep3D.SetActive(true);
                    break;
                }
            }
            yield return null;
        }

        m_isBroken = false;
    }
}
