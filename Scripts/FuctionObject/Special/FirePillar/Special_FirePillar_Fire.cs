using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_FirePilalr_ColorType { None = 0, Red, Blue }
public class Special_FirePillar_Fire : MonoBehaviour
{
    private static string m_collisionTag = "Special_FirePillar_Fire";
    private static string m_waterHeightPath = "_WaterHeight";

    [Header("[Fire Settings]")]
    [Space(-5f)]
    [Header("- Can Change")]

    /// <summary>시작 불 타입</summary>
    [SerializeField]
    private E_FirePilalr_ColorType m_startFireType;

    /// <summary>시작 불을 유지시킬 것인지 여부</summary>
    [SerializeField]
    private bool m_isKeepStartFire;

    /// <summary>물 증가 수치</summary>
    [SerializeField]
    private float m_waterIncreaseValue;

    [Header("- Don't Change")]
    /// <summary>불기둥</summary>
    [SerializeField]
    private Interaction_Push_FirePillar m_firePillar;

    /// <summary>불 오브젝트</summary>
    [SerializeField]
    private GameObject m_fireObject;

    /// <summary>빨간색 불</summary>
    [SerializeField]
    private GameObject m_redFire;
    /// <summary>파란색 불</summary>
    [SerializeField]
    private GameObject m_blueFire;

    [SerializeField]
    /// <summary>물 렌더러</summary>
    private MeshRenderer m_waterMeshRenderer;

    /// <summary>월드 오브젝트</summary>
    private WorldObject m_worldObject;
    /// <summary>콜라이더</summary>
    private Collider m_collider;
   
    private E_FirePilalr_ColorType m_currentFireColorType;
    /// <summary>현재 불 타입</summary>
    public E_FirePilalr_ColorType CurrentFireColorType { get { return m_currentFireColorType; } }

    /// <summary>충돌했었던 불 모음</summary>
    private Dictionary<Transform, Special_FirePillar_Fire> m_collisionFires;

    /// <summary>물 높이 설정 코루틴</summary>
    private Coroutine m_setWaterHeightCor;

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_collider = m_fireObject.GetComponent<Collider>();

        m_collisionFires = new Dictionary<Transform, Special_FirePillar_Fire>();

        InitFire();
    }

    /// <summary>불을 초기화 함</summary>
    private void InitFire()
    {
        m_redFire.SetActive(false);
        m_blueFire.SetActive(false);

        if (m_startFireType.Equals(E_FirePilalr_ColorType.Red))
            m_redFire.SetActive(true);
        else if (m_startFireType.Equals(E_FirePilalr_ColorType.Blue))
            m_blueFire.SetActive(true);

        // 저장
        m_currentFireColorType = m_startFireType;
    }

    private void Start()
    {
        StartCoroutine(SetCollider());
        StartCoroutine(SetRenderer());
    }

    /// <summary>시점에 따른 콜라이더 설정</summary>
    private IEnumerator SetCollider()
    {
        if (PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D))
            m_collider.enabled = false;

        WaitUntil isView3DWaitUntil = new WaitUntil(() => PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D));
        WaitUntil isView2DWaitUntil = new WaitUntil(() => PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D) && !PlayerManager.Instance.IsViewChange && m_worldObject.IsOnRenderer);

        while (true)
        {
            if (m_collider.enabled)
            {
                yield return isView3DWaitUntil;
                m_collider.enabled = false;
            }
            else
            {
                yield return isView2DWaitUntil;
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

    /// <summary>시점에 따른 렌더 설정</summary>
    private IEnumerator SetRenderer()
    {
        WaitUntil isView3DWaitUntil = new WaitUntil(() => PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D));
        WaitUntil isView2DWaitUntil = new WaitUntil(() => PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D) && !m_worldObject.IsOnRenderer);

        while(true)
        {
            yield return isView2DWaitUntil;
            m_fireObject.SetActive(false);
            yield return isView3DWaitUntil;
            m_fireObject.SetActive(true);
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

        // 각 타입에 맞춰 불 색 결정
        if (fireColorType.Equals(E_FirePilalr_ColorType.Red))
        {
            m_redFire.SetActive(true);
            m_blueFire.SetActive(false);
        }
        else if (fireColorType.Equals(E_FirePilalr_ColorType.Blue))
        {
            m_blueFire.SetActive(true);
            m_redFire.SetActive(false);
        }

        // 저장
        m_currentFireColorType = fireColorType;

        if (m_setWaterHeightCor != null)
            StopCoroutine(m_setWaterHeightCor);

        // 불기둥의 색과 불의 색이 같을경우 물이 차오르게 하기
        if (m_currentFireColorType.Equals(m_firePillar.FirePillarColorType))
            m_setWaterHeightCor = StartCoroutine(IncreaseWaterHeight());
        // 불기둥의 색과 불의 색이 다를경우 물이 감소하게 하기
        else
            m_setWaterHeightCor = StartCoroutine(DecreaseWaterHeight());
    }
    
    /// <summary>물의 높이를 증가</summary>
    private IEnumerator IncreaseWaterHeight()
    {
        float zero = 0f;
        float one = 1f;
        float value = m_waterMeshRenderer.material.GetFloat(m_waterHeightPath);

        while(true)
        {
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                value += m_waterIncreaseValue;
                value = Mathf.Clamp(value, zero, one);
                m_waterMeshRenderer.material.SetFloat(m_waterHeightPath, value);

                if (value.Equals(one))
                    break;
            }

            yield return null;
        }

        m_setWaterHeightCor = null;
    }

    /// <summary>물의 높이를 감소</summary>
    private IEnumerator DecreaseWaterHeight()
    {
        float zero = 0f;
        float one = 1f;
        float value = m_waterMeshRenderer.material.GetFloat(m_waterHeightPath);

        while(true)
        {
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                value -= m_waterIncreaseValue;
                value = Mathf.Clamp(value, zero, one);
                m_waterMeshRenderer.material.SetFloat(m_waterHeightPath, value);

                if (value.Equals(zero))
                    break;
            }

            yield return null;
        }

        m_setWaterHeightCor = null;
    }
}
