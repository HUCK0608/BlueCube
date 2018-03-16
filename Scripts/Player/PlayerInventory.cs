using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
    // 가지고 있는 열쇠 개수
    [SerializeField]
    private int m_key;
    /// <summary>열쇠를 하나 습득</summary>
    public void HaveKey() { m_key++;  Debug.Log("열쇠습득! 가지고 있는 열쇠 : " + m_key); }
    /// <summary>가지고 있는 열쇠 개수</summary>
    public int KeyAmount { get { return m_key; } }
    /// <summary>열쇠 사용</summary>
    public void UseKey(int amount) { m_key -= amount; Debug.Log("열쇠사용! 가지고 있는 열쇠 : " + m_key); }
}
