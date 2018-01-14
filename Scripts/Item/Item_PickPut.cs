using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_PickPut : MonoBehaviour
{
    // 아이템 스크립트
    private Item_PickPut2D m_item2D;
    private Item_PickPut3D m_item3D;

    // 소유중인지 체크
    private bool m_isHave;

    private void Awake()
    {
        m_item2D = transform.Find("2D").GetComponent<Item_PickPut2D>();
        m_item3D = transform.Find("3D").GetComponent<Item_PickPut3D>();
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
        // 고정 풀기
        m_isHave = false;

        Vector3 newPos = Vector3.zero;

        // 위치값
        float posX = m_item3D.transform.position.x;
        float posZ = m_item3D.transform.position.z;

        // 위치값 내림
        float floorX = Mathf.Floor(posX);
        float floorZ = Mathf.Floor(posZ);

        // x 좌표 계산
        float newPosX = 0f;

        // 내림한 값을 2로 나눈 나머지가 1인 경우
        if (floorX % 2 == 1)
            // 내림한 값에 1을 더함
            newPosX = floorX + 1;
        // 내림한 값을 2로 나눈 나머지가 -1인 경우
        else if (floorX % 2 == -1)
            // 내림한 값에 1을 뺌
            newPosX = floorX - 1;
        // 내림한 값을 2로 나눈 나머지가 0인 경우
        else
            // 내림한 값이 새로운 위치
            newPosX = floorX;

        // z 좌표 계산, 위와 동일한 계산
        float newPosZ = 0f;

        if (floorZ % 2 == 1)
            newPosZ = floorZ + 1;
        else
            newPosZ = floorZ;

        m_item3D.transform.position = new Vector3(newPosX, m_item3D.transform.position.y, newPosZ);

        // 중력키기
        m_item2D.OnGravity();
        m_item3D.OnGravity();
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
