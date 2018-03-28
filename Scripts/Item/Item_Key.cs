using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Key : MonoBehaviour
{
    /// <summary>플레이어에게 열쇠를 줌</summary>
    public void GiveKeyToPlayer()
    {
        // 수정(★)
        //PlayerManager.Instance.Inventory.HaveKey();
        gameObject.SetActive(false);
    }
}
