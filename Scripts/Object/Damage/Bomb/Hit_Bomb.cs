using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Hit_Bomb : MonoBehaviour
{
    // 폭탄 타이머
    [SerializeField]
    private float m_boomTimer;

    // 타이머가 켜져있을경우 hit 당할경우 뺄 시간
    [SerializeField]
    private float m_ifHitDecreaseTimer;

    // 충돌 이펙트 타입
    [SerializeField]
    private Effect_Type m_colEffectType;

    // 현재 타이머
    private float m_currentTimer;

    // 폭탄이 켜졌는지
    private bool m_isOn;

    // 폭탄이 켜졌을 때 쓸 임시 메테리얼
    private Material m_redMat;

    private void Awake()
    {
        m_redMat = Resources.Load("Materials/Dumy2") as Material;
    }

    // 폭탄 타격
    public void Hit()
    {
        if (m_isOn)
            m_currentTimer += m_ifHitDecreaseTimer;
        else
            StartCoroutine(OnTimer());
    }

    // 타이머 켜기
    private IEnumerator OnTimer()
    {
        m_isOn = true;

        // 메테리얼 변경
        transform.GetComponentInChildren<MeshRenderer>().material = m_redMat;

        while(true)
        {
            // 시점변환중이 아니고 2D가 아닐경우에만 실행
            if (!GameManager.Instance.PlayerManager.Skill_CV.IsChanging && !GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(GameLibrary.Enum_View2D))
            {
                m_currentTimer += Time.deltaTime;

                if (m_currentTimer >= m_boomTimer)
                    break;
            }
            yield return null;
        }

        Boom();
    }

    // 폭발
    private void Boom()
    {
        GameManager.Instance.EffectManager.CreateEffect(Effect_Type.Boom, transform.position);
        gameObject.SetActive(false);
    }
}
