using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tutorial_Move : Tutorial
{
    /// <summary>이동 UI 이미지</summary>
    [SerializeField]
    private Image m_moveUIImage;

    private void Start()
    {
        // 스킬을 사용하지 못하게 잠금
        PlayerManager.Instance.Skill.SetSkillLock(true);
    }

    protected override void StartTutorial()
    {
    }

    protected override void EndTutorial()
    {
    }
}
