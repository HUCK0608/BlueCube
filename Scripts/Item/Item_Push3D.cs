using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Push3D : MonoBehaviour
{
    // 얼마나 충돌되면 이동할건지 체크하는 시간
    [SerializeField]
    private float m_checkColTime;

    // 이동 속력
    [SerializeField]
    private float m_moveSpeed;

    // 충돌 태그명
    private string m_colTag;

    // 현재 충돌 체크중인지
    private bool m_isColCheck;

    // 현재 움직이는 중인지
    private bool m_isMove;

    // 메쉬 너비
    private float m_meshWidth;

    private void Awake()
    {
        m_colTag = "Player";

        m_meshWidth = GetComponent<MeshFilter>().mesh.bounds.extents.x;
    }

    private void OnCollisionStay(Collision other)
    {
        // 충돌체가 플레이어고 충돌 체크를 하지 않고 움직이지 않는 경우
        if(other.transform.tag == m_colTag && !m_isColCheck && !m_isMove)
        {
            // 충돌 체크 활성화
            m_isColCheck = true;

            // 플레이어 리지드바디
            Rigidbody playerRig = other.gameObject.GetComponent<Rigidbody>();

            // 충돌시간 체크 코루틴 시작
            StartCoroutine(CheckColTime(playerRig));
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.transform.tag == m_colTag)
        {
            m_isColCheck = false;
        }
    }

    // 얼마나 부딪혀있는지 체크하는 코루틴
    private IEnumerator CheckColTime(Rigidbody playerRig)
    {
        // 누적시간
        float accTime = 0f;


        while(m_isColCheck)
        {
            // 플레이어가 이동중일 경우만 시간 누적
            if (playerRig.velocity != Vector3.zero)
                accTime += Time.deltaTime;
            // 플레이어가 이동을 멈추면 누적시간 초기화
            else
                accTime = 0f;

            if(m_checkColTime <= accTime)
            {
                m_isMove = true;
                m_isColCheck = false;

                // 마지막 플레이어의 속도를 파라미터로 넘김
                StartCoroutine(Move(playerRig.velocity));
                break;
            }

            yield return null;
        }
    }

    private IEnumerator Move(Vector3 playerVelocity)
    {
        // x랑 z의 절대값 속도 비교
        float absX = Mathf.Abs(playerVelocity.x);
        float absZ = Mathf.Abs(playerVelocity.z);

        Vector3 destination = Vector3.zero;
        Vector3 rayDirection = Vector3.zero;

        // 부호
        float sign = 0;

        // x쪽의 속도가 더 클 경우
        if (absX > absZ)
        {
            // 부호체크
            if (playerVelocity.x > 0)
                sign = 1;
            else
                sign = -1;

            // 목적지 설정
            destination = transform.position + new Vector3(sign * 2, 0, 0);
            rayDirection = new Vector3(sign, 0, 0);
        }
        // z쪽의 속도가 더 클 경우
        else if(absX < absZ)
        {
            // 부호체크
            if (playerVelocity.z > 0)
                sign = 1;
            else
                sign = -1;

            // 목적지 설정
            destination = transform.position + new Vector3(0, 0, sign * 2);
            rayDirection = new Vector3(0, 0, sign);
        }
        else
        {
            m_isMove = false;
        }

        if (m_isMove)
        {
            Ray ray = new Ray(transform.position, rayDirection);

            // 나아갈 방향으로 레이를 쏴서 무언가 충돌되면 못가게 막음
            if (Physics.Raycast(ray, m_meshWidth * 2.9f * transform.parent.localScale.x))
                m_isMove = false;
            // 나아갈 방향에 아무것도 없으면 반대 방향으로 레이를 쏨
            else
            {
                ray = new Ray(transform.position, -rayDirection);

                RaycastHit hit;

                // 나아갈 방향 반대편으로 쏴서 플레이어가 있는지 체크
                if (Physics.Raycast(ray, out hit, m_meshWidth * 2f * transform.parent.localScale.x))
                {
                    // 플레이어가 없을 경우 이동하지 않음
                    if (hit.transform.tag != m_colTag)
                    {
                        m_isMove = false;
                    }
                }
                // 아무것도 없을 경우 이동하지 않음
                else
                    m_isMove = false;
            }
        }

        while (m_isMove)
        {
            // 이동
            transform.position = Vector3.MoveTowards(transform.position, destination, m_moveSpeed * Time.deltaTime);

            // 도착점에 도착하면 이동 코루틴 종료
            if(transform.position == destination)
            {
                m_isMove = false;
            }

            yield return null;
        }
    }
}
