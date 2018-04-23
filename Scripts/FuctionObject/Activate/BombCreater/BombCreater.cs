using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BombCreater : MonoBehaviour
{
    private WorldObject m_worldObject;
    private Activate m_activate;

    // 문
    [SerializeField]
    private Transform m_leftDoor, m_rightDoor;
    // 문 이동 속도
    [SerializeField]
    private float m_doorMoveSpeed;
    // 문 이동 거리
    [SerializeField]
    private float m_doorMoveDistance;

    // 폭탄 재활용을 위한 리스트 및 개수
    private List<GameObject> m_bombList;
    private int m_bombCount;
    // 폭탄
    [SerializeField]
    private GameObject m_bombPrefab;
    // 폭탄 이동 위치
    [SerializeField]
    private Transform m_bombMovePosition;
    // 현재 폭탄
    private Transform m_currentBomb;
    // 폭탄 이동 속도
    [SerializeField]
    private float m_bombMoveSpeed;

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_activate = GetComponent<Activate>();
        m_bombList = new List<GameObject>();
    }

    private void Start()
    {
        StartCoroutine(BombCreaterLogic());
    }

    /// <summary>폭탄 생성기 로직</summary>
    private IEnumerator BombCreaterLogic()
    {
        while (true)
        {
            // 스위치가 활성화 될 때 까지 기다림
            yield return new WaitUntil(() => m_activate.IsActivate);
            // 위에 아무것도 없을 때 까지 기다림
            yield return new WaitUntil(() => !GameLibrary.Raycast3D(transform.position, Vector3.up, m_bombMovePosition.position.y - transform.position.y, (-1) - GameLibrary.LayerMask_IgnoreRaycast));
            // 사용할 폭탄을 받아옴
            m_currentBomb = GetBomb().transform;
            // 문이 열릴 때 까지 기다림
            yield return StartCoroutine(OpenDoor());
            // 폭탄이 이동될 때 까지 기다림
            yield return StartCoroutine(BombMove());
            // 문이 닫힐 때 까지 기다림
            yield return StartCoroutine(CloseDoor());
        }
    }

    /// <summary>문 열기</summary>
    private IEnumerator OpenDoor()
    {
        Vector3 leftDoorDestination = m_leftDoor.position;
        leftDoorDestination.x -= m_doorMoveDistance;
        Vector3 rightDoorDestination = m_rightDoor.position;
        rightDoorDestination.x += m_doorMoveDistance;

        while(true)
        {
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                m_leftDoor.position = Vector3.MoveTowards(m_leftDoor.position, leftDoorDestination, m_doorMoveSpeed * Time.deltaTime);
                m_rightDoor.position = Vector3.MoveTowards(m_rightDoor.position, rightDoorDestination, m_doorMoveSpeed * Time.deltaTime);

                // 이동이 끝나면 반복문 종료
                if (m_leftDoor.position.Equals(leftDoorDestination))
                    break;
            }

            yield return null;
        }
    }

    /// <summary>폭탄 이동</summary>
    private IEnumerator BombMove()
    {
        while(true)
        {
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                m_currentBomb.position = Vector3.MoveTowards(m_currentBomb.position, m_bombMovePosition.position, m_bombMoveSpeed * Time.deltaTime);

                // 이동이 끝나면 반복문 종료
                if (m_currentBomb.position.Equals(m_bombMovePosition.position))
                    break;
            }

            yield return null;
        }
    }

    /// <summary>문 닫기</summary>
    private IEnumerator CloseDoor()
    {
        Vector3 leftDoorDestination = m_leftDoor.position;
        leftDoorDestination.x += m_doorMoveDistance;
        Vector3 rightDoorDestination = m_rightDoor.position;
        rightDoorDestination.x -= m_doorMoveDistance;

        while(true)
        {
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                m_leftDoor.position = Vector3.MoveTowards(m_leftDoor.position, leftDoorDestination, m_doorMoveSpeed * Time.deltaTime);
                m_rightDoor.position = Vector3.MoveTowards(m_rightDoor.position, rightDoorDestination, m_doorMoveSpeed * Time.deltaTime);

                // 이동이 끝나면 반복문 종료
                if (m_leftDoor.position.Equals(leftDoorDestination))
                    break;
            }

            yield return null;
        }
    }

    /// <summary>사용할 폭탄을 반환</summary>
    private GameObject GetBomb()
    {
        // 현재 사용할 수 있는 폭탄이 없다면 새로운 폭탄 생성
        if (m_bombCount.Equals(0))
            return CreateBomb();

        // 리스트에서 사용가능한 폭탄이 있는지 확인 후 반환
        for(int i = 0; i < m_bombCount; i++)
        {
            if (!m_bombList[i].activeSelf)
            {
                m_bombList[i].transform.position = transform.position;
                m_bombList[i].SetActive(true);
                return m_bombList[i];
            }
        }

        // 리스트에 사용 가능한 폭탄이 없을 경우 새로운 폭탄 생성
        return CreateBomb();
    }

    /// <summary>새로운 폭탄을 생성</summary>
    private GameObject CreateBomb()
    {
        GameObject newBomb = Instantiate(m_bombPrefab);

        newBomb.transform.parent = WorldManager.Instance.transform;
        newBomb.transform.position = transform.position;

        m_bombList.Add(newBomb);
        m_bombCount++;

        // 월드오브젝트에 포함
        WorldObject newWorldObject = newBomb.GetComponent<WorldObject>();
        WorldManager.Instance.AddWorldObject(newWorldObject);

        return newBomb;
    }
}
