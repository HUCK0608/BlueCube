using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_HpPostion2D : MonoBehaviour
{
    private Item_HpPostion m_hpPostion;

    private void Awake()
    {
        m_hpPostion = GetComponentInParent<Item_HpPostion>();
    }

     private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_hpPostion.PlayerHpIncrease();
        }
    }
}
