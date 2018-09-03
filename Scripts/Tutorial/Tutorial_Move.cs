using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tutorial_Move : Tutorial
{
    /// <summary>UI가 사라지는 시간</summary>
    [SerializeField]
    private float m_UIDisableTime;

    /// <summary>이동 UI 이미지</summary>
    private Image m_moveUIImage;

    private void Awake()
    {
        m_moveUIImage = GetComponent<Image>();
    }

    private void Start()
    {
        // 스킬을 사용하지 못하게 잠금
        PlayerManager.Instance.Skill.SetSkillLock(true);
        gameObject.SetActive(false);
    }

    public override void StartTutorial()
    {
        gameObject.SetActive(true);
        StartCoroutine(EndCheck());
    }

    /// <summary>튜토리얼 종료 체크</summary>
    private IEnumerator EndCheck()
    {
        yield return new WaitUntil(() => PlayerManager.Instance.MainController.CurrentState3D.Equals(E_PlayerState3D.Move));

        EndTutorial();
    }

    protected override void EndTutorial()
    {
        StartCoroutine(EndLogic());
    }

    /// <summary>종료 로직</summary>
    private IEnumerator EndLogic()
    {
        // 수치 계산
        float disableValue = 1.0f / m_UIDisableTime;

        Color moveUIColor = Color.white;
        float zero = 0.0f;
        float one = 1.0f;

        while(true)
        {
            moveUIColor.a = Mathf.Clamp(moveUIColor.a - disableValue * Time.deltaTime, zero, one);
            m_moveUIImage.color = moveUIColor;

            if (m_moveUIImage.color.a.Equals(zero))
                break;

            yield return null;
        }

        m_moveUIImage.gameObject.SetActive(false);
    }
}
