using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OutZone : MonoBehaviour
{
    /// <summary>플레이어에게 데미지를 입히고 리스폰시킴</summary>
    public void HitAndRespawnPlayer()
    {
        PlayerManager.Instance.Hit(1);
        GameManager.Instance.RestartLevel();
    }
}
