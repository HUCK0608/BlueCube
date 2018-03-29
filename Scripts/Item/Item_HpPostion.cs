using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_HpPostion : MonoBehaviour
{
    // 체력증가량
    [SerializeField]
    private int m_hpIncreaseAmount;

    /// <summary>플레이어 체력 증가시키기</summary>
    public void PlayerHpIncrease()
    {
        // 수정(★)
        //PlayerManager.Instance.Stat.HpIncrease(m_hpIncreaseAmount);
        gameObject.SetActive(false);
    }
}
