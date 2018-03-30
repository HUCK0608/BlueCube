using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private int m_currentHaveKey;
    /// <summary>플레이어에게 열쇠를 1개 줌</summary>
    public void GiveKey() { m_currentHaveKey += 1; }
    /// <summary>현재 가지고 있는 열쇠의 개수가 요구하는 개수를 만족할경우 true를 반환하며 열쇠를 사용</summary>
    public bool UseKey(int amount)
    {
        if (m_currentHaveKey >= amount)
        {
            m_currentHaveKey -= amount;
            return true;
        }

        return false;
    }
    
}
