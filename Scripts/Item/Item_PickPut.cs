using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_PickPut : MonoBehaviour
{
    // 아이템
    private Transform m_item;

    // 3D 리지드바디
    private Rigidbody m_rigidbody3D;

    // 아이템을 플레이어가 소유중인지
    private bool m_isHave;

    private void Awake()
    {
        m_item = transform.Find("Collider3D");

        m_rigidbody3D = m_item.GetComponent<Rigidbody>();
    }

    // 중력 사용 여부
    private void UseGravity(bool value)
    {
        m_rigidbody3D.useGravity = value;
    }

    public void PickUp(Transform playerHand2D, Transform playerHand3D)
    {
        
    }

    private IEnumerator FixedItem()
    {
        m_isHave = true;

        // 중력 끄기
        UseGravity(false);

        while(m_isHave)
        {
            yield return null;
        }
    }

    // 놓기
    public void Put()
    {
        // 고정 풀기
        m_isHave = false;

        // 위치값
        float posX = m_item.position.x;
        float posZ = m_item.position.z;

        // 위치값 내림
        float floorX = Mathf.Floor(posX);
        float floorZ = Mathf.Floor(posZ);

        Vector3 newPos = Vector3.zero;

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

        newPos.x = newPosX;
        newPos.y = m_item.transform.position.y;
        newPos.z = newPosZ;

        // 정규화 위치 이동
        m_item.position = newPos;

        // 중력사용
        UseGravity(true);
    }
}
