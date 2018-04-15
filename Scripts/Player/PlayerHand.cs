using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerHand : MonoBehaviour
{
    [SerializeField]
    private Transform m_pickItemPosition3D;
    /// <summary>3D 아이템 잡는 위치를 반환</summary>
    public Transform PickObjectPoint { get { return m_pickItemPosition3D; } }

    private Item_PickPut m_currentPickItem;
    /// <summary>현재 들고 있는 아이템</summary>
    public Item_PickPut CurrentPickItem { get { return m_currentPickItem; }  set { m_currentPickItem = value; } }

    private Interaction_PickPut m_currentPickPutObject;
    /// <summary>현재 상호작용 중인 들고놓기 오브젝트</summary>
    public Interaction_PickPut CurrentPickPutObject { get { return m_currentPickPutObject; } set { m_currentPickPutObject = value; } }

    private Interaction_Push m_currentPushItem;
    /// <summary>현재 밀고 있는 아이템</summary>
    public Interaction_Push CurrentPushItem { get { return m_currentPushItem; } set { m_currentPushItem = value; } }
}
