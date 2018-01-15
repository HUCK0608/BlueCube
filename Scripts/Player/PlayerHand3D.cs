using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerHand3D : MonoBehaviour
{
    // 매니저
    private PlayerManager m_manager;

    // 각 손
    private Transform m_playerHand2D;
    private Transform m_playerHand3D;

    // 아이템 체크하는 위치
    private Transform m_checkItem3D;

    // 들고있는 아이템
    private Item_PickPut m_haveItem;

    private void Awake()
    {
        m_manager = transform.parent.parent.GetComponent<PlayerManager>();

        m_playerHand2D = transform.parent.parent.Find("2D").Find("PlayerHand");
        m_playerHand3D = transform;

        m_checkItem3D = transform.GetChild(0);
    }

    private void Update()
    {
        InteractionItem();
    }

    // 아이템 상호작용
    private void InteractionItem()
    {
        // 상호작용키를 눌렀을경우
        if(Input.GetKeyDown(m_manager.InteractionKey))
        {
            // 현재 아이템이 없다면 아이템 줍기
            if (m_haveItem == null)
                PickUpItem();
            // 현재 아이템이 있다면 아이템 놓기
            else if (m_haveItem != null)
                PutItem();
        }
    }

    // 아이템 들기
    private void PickUpItem()
    {
        Ray ray = new Ray(m_checkItem3D.position, transform.forward.normalized);
        RaycastHit hit;
        // 아이템레이어만 충돌되게 설정
        int layerMask = 1 << 9;
        // 레이에 충돌된 아이템이 있을경우
        if (Physics.Raycast(ray, out hit, 2f, layerMask))
        {
            m_haveItem = hit.transform.parent.GetComponent<Item_PickPut>();
            // 아이템 줍기
            m_haveItem.PickUp(m_playerHand2D, m_playerHand3D);
        }
    }

    // 아이템 놓기
    private void PutItem()
    {
        // 아이템 놓기
        m_haveItem.Put();
        // 초기화
        m_haveItem = null;
    }

}
