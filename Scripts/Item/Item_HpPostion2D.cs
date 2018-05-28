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
            EffectManager.Instance.CreateEffect(Effect_Type.Player_Heal, PlayerManager.Instance.Player2D_Object.transform.position);
            m_hpPostion.PlayerHpIncrease();
        }
    }
}
