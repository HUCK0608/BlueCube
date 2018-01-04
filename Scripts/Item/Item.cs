﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item : MonoBehaviour
{
    // 아이템 스크립트
    private Item2D m_item2D;
    private Item3D m_item3D;

    // 소유중인지 체크
    private bool m_isHave;

    private void Awake()
    {
        m_item2D = transform.Find("2D").GetComponent<Item2D>();
        m_item3D = transform.Find("3D").GetComponent<Item3D>();
    }

    // 줍기
    public void PickUp(Transform playerHand2D, Transform playerHand3D)
    {
        // 중력끄기
        m_item2D.OffGravity();
        m_item3D.OffGravity();

        // 플레이어 고정 코루틴 실행
        StartCoroutine(FixedPlayerHand(playerHand2D, playerHand3D));
    }

    // 놓기
    public void Put()
    {
        // 중력키기
        m_item2D.OnGravity();
        m_item3D.OnGravity();
        m_isHave = false;
    }

    // 플레이어에게 아이템 고정시키기
    private IEnumerator FixedPlayerHand(Transform playerHand2D, Transform playerHand3D)
    {
        m_isHave = true;

        // 소유중일 경우만 루프
        while(m_isHave)
        {
            // 2D일 경우
            if(GameManager.Instance.ViewType == E_ViewType.View2D)
            {
                m_item2D.transform.position = playerHand2D.position;
            }
            // 3D일 경우
            else if(GameManager.Instance.ViewType == E_ViewType.View3D)
            {
                m_item3D.transform.position = playerHand3D.position;
            }

            yield return null;
        }
    }
}
