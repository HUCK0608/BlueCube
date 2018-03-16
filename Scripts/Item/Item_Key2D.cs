using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Key2D : MonoBehaviour
{
    private Item_Key m_key;

    private void Awake()
    {
        m_key = GetComponentInParent<Item_Key>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_key.GiveKeyToPlayer();
        }
    }
}
