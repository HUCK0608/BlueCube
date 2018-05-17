using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Push_FirePillar : Interaction_Push
{
    [Space(20f)]
    [Header("* FirePillar Settings")]
    [Space(-5f)]
    [Header("Can Change")]

    /// <summary>불이 켜졌는지 꺼졌는지 여부</summary>
    [SerializeField]
    private bool m_isOnFire;

    [Header("Don't touch")]

    /// <summary>불 오브젝트</summary>
    [SerializeField]
    private GameObject m_fire;

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>pushPosition으로 밀 수 있을경우 true를 반환(FirePillar 방식)</summary>
    protected override bool CheckCanPush(Vector3 pushPosition)
    {
        // 기존에 있던 함수의 계산된 값을 가져옴
        bool baseValue = base.CheckCanPush(pushPosition);

        // 밀 수 없다면 그대로 밀 수 없다고 전달
        if (!baseValue)
            return false;
        // 기존에 계산된 것에 밀 수 있다면 아래 계산을 더 계산함
        else
        {
            return true;
        }
    }
}
