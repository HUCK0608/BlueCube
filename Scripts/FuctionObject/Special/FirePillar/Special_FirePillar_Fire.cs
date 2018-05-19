using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_FirePilalr_ColorType { None = 0, Red, Blue }
public class Special_FirePillar_Fire : MonoBehaviour
{
    private static string m_collisionTag = "Special_FirePillar_Fire";

    [Header("[Fire Settings]")]
    [Space(-5f)]
    [Header("- Can Change")]

    /// <summary>시작 불 타입</summary>
    [SerializeField]
    private E_FirePilalr_ColorType m_startFireType;

    /// <summary>시작 불을 유지시킬 것인지 여부</summary>
    [SerializeField]
    private bool m_isKeepStartFire;

    [Header("- Don't Change")]
    /// <summary>불 오브젝트</summary>
    [SerializeField]
    private GameObject m_fireObject;

    /// <summary>월드 오브젝트</summary>
    private WorldObject m_worldObject;
    /// <summary>불 메쉬 렌더러</summary>
    private MeshRenderer m_meshRenderer;
    /// <summary>불 메테리얼</summary>
    private Material m_material;
    /// <summary>콜라이더</summary>
    private Collider m_collider;

    private E_FirePilalr_ColorType m_currentFireColorType;
    /// <summary>현재 불 타입</summary>
    public E_FirePilalr_ColorType CurrentFireColorType { get { return m_currentFireColorType; } }

    /// <summary>충돌했었던 불 모음</summary>
    private Dictionary<Transform, Special_FirePillar_Fire> m_collisionFires;

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_meshRenderer = m_fireObject.GetComponent<MeshRenderer>();
        m_material = m_meshRenderer.material;
        m_collider = m_fireObject.GetComponent<Collider>();

        m_collisionFires = new Dictionary<Transform, Special_FirePillar_Fire>();

        InitFire();
    }

    /// <summary>불을 초기화 함</summary>
    private void InitFire()
    {
        // 각 타입에 맞춰 불을 초기화
        if (m_startFireType.Equals(E_FirePilalr_ColorType.None))
            m_meshRenderer.enabled = false;
        else if (m_startFireType.Equals(E_FirePilalr_ColorType.Red))
            m_material.color = Color.red;
        else if (m_startFireType.Equals(E_FirePilalr_ColorType.Blue))
            m_material.color = Color.blue;

        // 저장
        m_currentFireColorType = m_startFireType;
    }

    private void Start()
    {

        StartCoroutine(SetCollider());
        StartCoroutine(SetRendererEnable());
    }

    /// <summary>시점에 따른 콜라이더 2D 설정</summary>
    private IEnumerator SetCollider()
    {
        if (PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D))
            m_collider.enabled = false;

        WaitUntil m_isView3DWaitUntil = new WaitUntil(() => PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D));
        WaitUntil m_isView2DWaitUntil = new WaitUntil(() => PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D) && !PlayerManager.Instance.IsViewChange && m_worldObject.IsOnRenderer);

        while (true)
        {
            if (m_collider.enabled)
            {
                yield return m_isView3DWaitUntil;
                m_collider.enabled = false;
            }
            else
            {
                yield return m_isView2DWaitUntil;
                m_collider.enabled = true;

                // 한 프레임을 기다림
                yield return null;

                // z음의 방향에 불 체크를 하고 없을 경우 현재 불이 없을 경우에만 양의 방향도 체크한다.
                if (!CheckAnotherFire_BackDirection() && CurrentFireColorType.Equals(E_FirePilalr_ColorType.None))
                    CheckAnotherFire_FrontDirection();
            }

            yield return null;
        }
    }

    /// <summary>시점에 따른 랜더러 설정</summary>
    private IEnumerator SetRendererEnable()
    {
        WaitUntil m_isView3DWaitUntil = new WaitUntil(() => PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D) && !m_meshRenderer.enabled && !CurrentFireColorType.Equals(E_FirePilalr_ColorType.None));
        WaitUntil m_isView2DWaitUntil = new WaitUntil(() => PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D) && !m_worldObject.IsOnRenderer && m_meshRenderer.enabled);

        while (true)
        {
            yield return m_isView2DWaitUntil;
            m_meshRenderer.enabled = false;
            yield return m_isView3DWaitUntil;
            m_meshRenderer.enabled = true;
        }
    }

    /// <summary>z음의 방향으로 다른 불이 있는지 체크 후 불 타입 변경</summary>
    private bool CheckAnotherFire_BackDirection()
    {
        // 처음 불을 유지하는 경우 리턴
        if (m_isKeepStartFire)
            return true;

        RaycastHit[] collisionObjects = GameLibrary.RaycastAll3D(m_fireObject.transform.position, Vector3.back, Mathf.Infinity);
        int collisionObjectsLength = collisionObjects.Length;

        // 충돌된 오브젝트가 하나라도 있을 경우 실행
        if (!collisionObjects.Length.Equals(0))
        {
            int farIndex = -1;
            float farDistance = 0f;

            for (int i = 0; i < collisionObjects.Length; i++)
            {
                // 오브젝트가 불 오브젝트일 경우
                if (collisionObjects[i].transform.tag.Equals(m_collisionTag))
                {
                    // 이 불이 충돌되었던 불 리스트에 존재하지 않으면 리스트에 추가
                    if (!m_collisionFires.ContainsKey(collisionObjects[i].transform))
                        m_collisionFires.Add(collisionObjects[i].transform, collisionObjects[i].transform.GetComponentInParent<Special_FirePillar_Fire>());

                    // 충돌 불 리스트에서 지금 이 불을 찾아 불이 꺼져있지 않은 경우에만 거리를 체크하여 저장
                    if (!m_collisionFires[collisionObjects[i].transform].CurrentFireColorType.Equals(E_FirePilalr_ColorType.None))
                    {
                        float distance = Vector3.Distance(m_fireObject.transform.position, collisionObjects[i].transform.position);

                        // 이 불까지의 거리가 기존 불의 거리보다 멀 경우
                        if (distance > farDistance)
                        {
                            farDistance = distance;
                            farIndex = i;
                        }
                    }
                }
            }

            // 먼 쪽의 불의 인덱스가 있을 경우에만 나의 불을 z음의 방향으로 제일 먼 불로 변경
            if (!farIndex.Equals(-1))
            {
                SetChangeFire(m_collisionFires[collisionObjects[farIndex].transform].CurrentFireColorType);
                return true;
            }
        }

        return false;
    }

    /// <summary>z양의 방향으로 다른 불이 있는지 체크 후 불 타입 변경</summary>
    private void CheckAnotherFire_FrontDirection()
    {
        RaycastHit[] collisionObjects = GameLibrary.RaycastAll3D(m_fireObject.transform.position, Vector3.forward, Mathf.Infinity);
        int collisionObjectsLength = collisionObjects.Length;

        // 충돌된 오브젝트가 하나라도 있을 경우 실행
        if (!collisionObjectsLength.Equals(0))
        {
            int nearIndex = -1;
            float nearDistance = 1000f;

            for (int i = 0; i < collisionObjectsLength; i++)
            {
                if (collisionObjects[i].transform.tag.Equals(m_collisionTag))
                {
                    // 이 불이 충돌되었던 불 리스트에 존재하지 않으면 리스트에 추가
                    if (!m_collisionFires.ContainsKey(collisionObjects[i].transform))
                        m_collisionFires.Add(collisionObjects[i].transform, collisionObjects[i].transform.GetComponentInParent<Special_FirePillar_Fire>());

                    // 충돌 불 리스트에서 지금 이 불을 찾아 불이 꺼져있지 않은 경우에만 거리를 체크하여 저장
                    if (!m_collisionFires[collisionObjects[i].transform].CurrentFireColorType.Equals(E_FirePilalr_ColorType.None))
                    {
                        float distance = Vector3.Distance(m_fireObject.transform.position, collisionObjects[i].transform.position);

                        // 이 불까지의 거리가 기존 불의 거리보다 가까울 경우
                        if (distance < nearDistance)
                        {
                            nearDistance = distance;
                            nearIndex = i;
                        }
                    }
                }
            }

            if (!nearIndex.Equals(-1))
                SetChangeFire(m_collisionFires[collisionObjects[nearIndex].transform].CurrentFireColorType);
        }
    }

    /// <summary>fireType불로 바꿈</summary>
    private void SetChangeFire(E_FirePilalr_ColorType fireColorType)
    {
        // fireType과 currentFireType이 같을 경우 리턴
        if (fireColorType.Equals(m_currentFireColorType))
            return;

        // 랜더러가 꺼져있다면 활성화
        if (!m_meshRenderer.enabled)
            m_meshRenderer.enabled = true;

        // 각 타입에 맞춰 불 색 결정
        if (fireColorType.Equals(E_FirePilalr_ColorType.Red))
            m_material.color = Color.red;
        else if (fireColorType.Equals(E_FirePilalr_ColorType.Blue))
            m_material.color = Color.blue;

        // 저장
        m_currentFireColorType = fireColorType;
    }
}
