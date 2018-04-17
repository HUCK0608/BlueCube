using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_Bomb : MonoBehaviour
{
    // 모델
    private Transform m_model;
    // 리지드바디
    private Rigidbody m_rigidbody;
    // 메쉬렌더러
    private MeshRenderer m_meshRenderer;

    // 폭탄 타이머
    [SerializeField]
    private float m_bombTimer;

    private void Awake()
    {
        m_model = transform.Find("ModelAndCollider3D");
        m_rigidbody = GetComponentInChildren<Rigidbody>();
        m_meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        StartCoroutine(BombLogic());
    }

    private IEnumerator BombLogic()
    {
        // 오브젝트를 놓을 때 까지 기다림
        yield return StartCoroutine(WaitPutEnd());
        // 오브젝트가 충돌할 때 까지 기다림
        yield return StartCoroutine(WaitCollision());

        m_meshRenderer.material = GameLibrary.Material_Red;

        // 터질 시간까지 기다림
        yield return new WaitForSeconds(m_bombTimer);

        // 현재 위치에 폭발 이펙트 생성
        EffectManager.Instance.CreateEffect(Effect_Type.Boom, m_model.position);

        // 비활성화
        gameObject.SetActive(false);
    }

    // 오브젝트를 놓을 때 까지 기다림
    private IEnumerator WaitPutEnd()
    {
        while(m_rigidbody.isKinematic) yield return null; 
    }

    // 오브젝트가 충돌할 때 까지 기다림
    private IEnumerator WaitCollision()
    {
        while(!m_rigidbody.isKinematic) yield return null;
    }
}
