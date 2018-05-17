using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_FirePillar_FireType { None = 0, Red, Blue }
public sealed class Special_FirePillar_Fire : MonoBehaviour
{
    private static string m_collisionTag = "Special_FirePillar_Fire";

    [Header("Can Change")]

    /// <summary>시작 불 타입</summary>
    [SerializeField]
    private E_FirePillar_FireType m_startFireType;

    /// <summary>시작 불을 유지시킬 것인지 여부</summary>
    [SerializeField]
    private bool m_isKeepStartFire;

    /// <summary>불 메쉬 렌더러</summary>
    private MeshRenderer m_meshRenderer;
    /// <summary>불 메테리얼</summary>
    private Material m_material;

    private E_FirePillar_FireType m_currentFireType;
    /// <summary>현재 불 타입</summary>
    public E_FirePillar_FireType CurrentFireType { get { return m_currentFireType; } }

    private Vector3 m_fireSourcePosition;
    /// <summary>시작부터 가지고 있던 불이 아닐경우 불의 근원 위치</summary>
    public Vector3 FireSourcePosition { get { return m_fireSourcePosition; } }

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_material = m_meshRenderer.material;

        InitFire();
    }

    /// <summary>불을 초기화 함</summary>
    private void InitFire()
    {
        // 각 타입에 맞춰 불을 초기화
        if (m_startFireType.Equals(E_FirePillar_FireType.None))
            m_meshRenderer.enabled = false;
        else if (m_startFireType.Equals(E_FirePillar_FireType.Red))
            m_material.color = Color.red;
        else if (m_startFireType.Equals(E_FirePillar_FireType.Blue))
            m_material.color = Color.blue;

        // 저장
        m_currentFireType = m_startFireType;
    }

    /// <summary>fireType불로 바꿈</summary>
    private void SetChangeFire(E_FirePillar_FireType fireType)
    {
        // fireType과 currentFireType이 같을 경우 리턴
        if (fireType.Equals(m_currentFireType))
            return;

        // 랜더러가 꺼져있다면 활성화
        if (!m_meshRenderer.enabled)
            m_meshRenderer.enabled = true;

        // 각 타입에 맞춰 불 색 결정
        if (fireType.Equals(E_FirePillar_FireType.Red))
            m_material.color = Color.red;
        else if (fireType.Equals(E_FirePillar_FireType.Blue))
            m_material.color = Color.blue;

        // 저장
        m_currentFireType.Equals(fireType);
    }

    /// <summary>2D 트리거 충돌</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 태그가 일치하고 시작 불을 유지하지 않을 경우 불을 변경 시킨다.
        if(other.tag.Equals(m_collisionTag) && !m_isKeepStartFire)
        {
            Special_FirePillar_Fire anotherFire = other.GetComponent<Special_FirePillar_Fire>();

            Debug.Log("call");
        }
    }
}
