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
    private Item m_haveItem;

    private void Awake()
    {
        m_manager = transform.parent.parent.GetComponent<PlayerManager>();

        m_playerHand2D = transform.parent.parent.Find("2D").Find("PlayerHand");
        m_playerHand3D = transform;

        m_checkItem3D = transform.GetChild(0);
    }

    private void Update()
    {
        PutItem();
        PickUpItem();
    }

    // 아이템 들기
    private void PickUpItem()
    {
        // 상호작용 키를 눌렀을 경우
        if (Input.GetKeyDown(m_manager.InteractionKey))
        {
            // 들고있는 아이템이 없을 경우
            if (m_haveItem == null)
            {
                Ray ray = new Ray(m_checkItem3D.position, transform.forward.normalized);
                RaycastHit hit;
                // 아이템레이어만 충돌되게 설정
                int layerMask = 1 << 8;
                // 레이에 충돌된 아이템이 있을경우
                if (Physics.Raycast(ray, out hit, 0.7f, layerMask))
                {
                    m_haveItem = hit.transform.parent.GetComponent<Item>();
                    // 아이템 줍기
                    m_haveItem.PickUp(m_playerHand2D, m_playerHand3D);
                }
            }
        }
    }

    // 아이템 놓기
    private void PutItem()
    {
        if(Input.GetKeyDown(m_manager.InteractionKey))
        {
            // 들고있는 아이템이 있을 경우
            if(m_haveItem != null)
            {
                // 아이템 놓기
                m_haveItem.Put();
                // 초기화
                m_haveItem = null;
            }
        }
    }
}
