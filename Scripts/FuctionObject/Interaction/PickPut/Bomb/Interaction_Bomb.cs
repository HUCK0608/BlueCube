﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_Bomb : MonoBehaviour
{
    private WorldObject m_worldObject;
    // 모델
    private Transform m_model;
    // 리지드바디
    private Rigidbody m_rigidbody;

    [Header("Don't Touch !")]

    [SerializeField]
    private GameObject m_effects;

    [Header("Can Change")]

    // 폭탄 타이머
    [SerializeField]
    private float m_bombTimer;

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_model = transform.Find("ModelAndCollider3D");
        m_rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(BombLogic());
    }

    private IEnumerator BombLogic()
    {
        // 오브젝트를 놓을 때 까지 기다림
        yield return new WaitUntil(() => !m_rigidbody.isKinematic);
        // 오브젝트가 충돌할 때 까지 기다림
        yield return new WaitUntil(() => m_rigidbody.isKinematic);

        m_effects.SetActive(true);

        // 폭탄 타이머를 기다림
        yield return StartCoroutine(BombTimer());

        // 현재 위치에 폭발 이펙트 생성
        EffectManager.Instance.CreateEffect(Effect_Type.Player_Boom, m_model.position);

        m_effects.SetActive(false);

        // 위치 초기화
        m_model.localPosition = Vector3.zero;

        // 비활성화
        gameObject.SetActive(false);
    }

    /// <summary>폭탄 타이머</summary>
    private IEnumerator BombTimer()
    {
        float addTime = 0f;

        while(true)
        {
            if(!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                addTime += Time.deltaTime;

                if (addTime >= m_bombTimer)
                    break;
            }

            yield return null;
        }
    }
}
