using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerHand : MonoBehaviour
{
    [SerializeField]
    private Transform m_pickItemPosition3D;
    /// <summary>3D 아이템 잡는 위치를 반환</summary>
    public Transform PickItemPosition3D { get { return m_pickItemPosition3D; } }

    private Item_PickPut m_currentPickItem;
    /// <summary>현재 들고 있는 아이템</summary>
    public Item_PickPut CurrentPickItem { get { return m_currentPickItem; }  set { m_currentPickItem = value; } }

    private Item_Push m_currentPushItem;
    /// <summary>현재 밀고 있는 아이템</summary>
    public Item_Push CurrentPushItem { get { return m_currentPushItem; } set { m_currentPushItem = value; } }
}
