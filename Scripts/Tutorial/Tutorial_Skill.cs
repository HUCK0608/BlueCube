using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Tutorial_Skill : Tutorial
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public override void StartTutorial()
    {
        // 스킬 잠금 해제
        PlayerManager.Instance.Skill.SetSkillLock(false);
        gameObject.SetActive(true);

        StartCoroutine(EndCheck());
    }

    /// <summary>튜토리얼 종료 체크</summary>
    private IEnumerator EndCheck()
    {
        // 시점변환중일때까지 대기
        yield return new WaitUntil(() => PlayerManager.Instance.IsViewChange);

        EndTutorial();
    }

    protected override void EndTutorial()
    {
        gameObject.SetActive(false);
    }
}
