using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Hit_Bomb2D : MonoBehaviour
{
    private Hit_Bomb m_bomb;

    private void Awake()
    {
        m_bomb = GetComponentInParent<Hit_Bomb>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_PlayerAttack))
        {
            m_bomb.Hit();
        }
    }
}
