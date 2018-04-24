using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_HpPostion3D : MonoBehaviour
{
    private Item_HpPostion m_hpPostion;

    private void Awake()
    {
        m_hpPostion = GetComponentInParent<Item_HpPostion>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameLibrary.String_Player))
        {
            EffectManager.Instance.CreateEffect(Effect_Type.Heal, PlayerManager.Instance.Player3D_Object.transform.position);
            m_hpPostion.PlayerHpIncrease();
        }
    }
}
