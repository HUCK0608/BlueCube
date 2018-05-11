using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Story2D : MonoBehaviour
{
    private Item_Story m_stroy;

    private void Awake()
    {
        m_stroy = GetComponentInParent<Item_Story>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_stroy.GetStory();
        }
    }
}
