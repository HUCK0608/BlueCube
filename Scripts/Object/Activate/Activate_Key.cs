using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Activate_Key : Activate
{
    // 필요한 키 개수
    [SerializeField]
    private int m_needKeyAmount;

    private Activate_KeyCheckBox m_keyCheckBox;

    private void Awake()
    {
        m_keyCheckBox = GetComponentInChildren<Activate_KeyCheckBox>();
    }

    public void KeyCheck()
    {
        // 수정(★)
        //PlayerInventory playerInventory = PlayerManager.Instance.Inventory;
        //int playerKeyAmount = playerInventory.KeyAmount;

        //if (playerKeyAmount >= m_needKeyAmount)
        //{
        //    playerInventory.UseKey(m_needKeyAmount);
        //    m_isActivate = true;
        //    m_keyCheckBox.gameObject.SetActive(false);
        //}
    }
}
