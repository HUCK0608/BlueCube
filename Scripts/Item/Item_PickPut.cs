using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_PickPut : MonoBehaviour
{
    // 들었을 때 떨어질 위치 표시를 위한 오브젝트
    private static Transform m_fadeBox;

    // 3D 리지드바디
    private Rigidbody m_rigidbody3D;

    // 아이템을 플레이어가 소유중인지
    private bool m_isHave;

    // 들었을 때 떨어질 위치표시를 위한 변수
    private Ray m_ray;
    private RaycastHit m_hit;

    private void Awake()
    {
        if(m_fadeBox == null)
            m_fadeBox = GameObject.Find("Item_Box_PickPut_Fade").transform;

        m_rigidbody3D = GetComponent<Rigidbody>();

        m_ray = new Ray();
        m_ray.direction = Vector3.down;
    }

    // 중력 사용 여부
    private void UseGravity(bool value)
    {
        m_rigidbody3D.useGravity = value;
    }

    // 들기
    public void PickUp(Transform playerHand2D, Transform playerHand3D)
    {
        StartCoroutine(FixedItem(playerHand2D, playerHand3D));
    }

    // 플레이어 손에 고정
    private IEnumerator FixedItem(Transform playerHand2D, Transform playerHand3D)
    {
        m_isHave = true;

        // 중력 끄기
        UseGravity(false);

        // fadeBox 활성화
        m_fadeBox.gameObject.SetActive(true);

        while(m_isHave)
        {
            if (GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(E_ViewType.View2D))
                transform.position = playerHand2D.position;
            else if (GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(E_ViewType.View3D))
                transform.position = playerHand3D.position;

            // fadeBox 그리기
            DrawFadeBox();

            yield return null;
        }

        m_fadeBox.gameObject.SetActive(false);
    }

    /// <summary>FadeBox 그리기</summary>
    private void DrawFadeBox()
    {
        Vector3 putPos = CalcPutPos();

        m_ray.origin = putPos;

        if(Physics.Raycast(m_ray, out m_hit, Mathf.Infinity, GameLibrary.LayerMask_Ignore_BP))
        {
            putPos.y = m_hit.point.y + 1f;

            m_fadeBox.position = putPos;
        }
    }

    /// <summary>아이템 놓기</summary>
    public void Put()
    {
        // 고정 풀기
        m_isHave = false;

        // 떨어질 위치 받아오기
        Vector3 putPos = CalcPutPos();

        // 정규화 위치 이동
        transform.position = putPos;

        // 중력사용
        UseGravity(true);
    }

    /// <summary>떨어질 위치 계산</summary>
    private Vector3 CalcPutPos()
    {
        // 위치값
        float posX = transform.position.x;
        float posZ = transform.position.z;

        // 위치값 내림
        float floorX = Mathf.Floor(posX);
        float floorZ = Mathf.Floor(posZ);

        Vector3 putPos = Vector3.zero;

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

        putPos.x = newPosX;
        putPos.y = transform.transform.position.y;
        putPos.z = newPosZ;

        return putPos;
    }
}
