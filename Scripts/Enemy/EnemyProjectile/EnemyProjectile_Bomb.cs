using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyProjectile_Bomb : EnemyProjectile
{
    private Collider m_collider;
    private MeshRenderer m_meshRenderer;
    private Material m_startMaterial;

    // 현재 속도
    private Vector3 m_currentVelocity;

    // 최종 시간
    [SerializeField]
    private float m_finalTime;

    // 중력
    [SerializeField]
    private float m_gravity;

    // 터지기까지 시간
    [SerializeField]
    private float m_bombTimer;

    protected override void Awake()
    {
        base.Awake();

        m_collider = GetComponentInChildren<Collider>();
        m_meshRenderer = GetComponentInChildren<MeshRenderer>();
        m_startMaterial = m_meshRenderer.material;
    }

    public override void UseProjectile(Vector3 origin, Vector3 destination)
    {
        base.UseProjectile(origin, destination);

        StartCoroutine(StartProjectileLogic(origin, destination));
    }

    /// <summary>투사체 로직 실행</summary>
    private IEnumerator StartProjectileLogic(Vector3 origin, Vector3 destination)
    {
        // 투사체를 처음 위치로 이동
        m_rigidbody.transform.position = origin;

        // 처음 속도 계산
        m_currentVelocity = CalcStartVelocity(destination);

        // 이동 코루틴을 기다림
        yield return StartCoroutine(Move());

        // 폭탄 로직을 기다림
        yield return StartCoroutine(BombLogic());

        // 투사체 사용 중지
        UseStopProjectile();
    }

    /// <summary>destination까지 가기 위한 처음 속도 계산</summary>
    private Vector3 CalcStartVelocity(Vector3 destination)
    {
        Vector3 startVelocity = Vector3.zero;

        Vector3 thisPosition = m_rigidbody.transform.position;

        float distanceX = destination.x - thisPosition.x;
        float distanceY = destination.y - thisPosition.y;
        float distanceZ = destination.z - thisPosition.z;

        // 등속도부터 계산
        startVelocity.x = distanceX / m_finalTime;
        startVelocity.z = distanceZ / m_finalTime;

        // 가속도를 이용하여 y속도 계산
        startVelocity.y = (distanceY / m_finalTime) - (0.5f * m_gravity * m_finalTime);

        return startVelocity;
    }

    /// <summary>이동</summary>
    private IEnumerator Move()
    {
        m_rigidbody.isKinematic = false;

        float stopDistanceY = m_collider.bounds.extents.y + 0.2f;
        int layerMask = (-1) - (GameLibrary.LayerMask_BackgroundTrigger |
                                     GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_IgnoreRaycast);

        while(true)
        {
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                m_currentVelocity.y = m_currentVelocity.y + m_gravity * Time.deltaTime;
                m_rigidbody.velocity = m_currentVelocity;

                // 3D일 경우
                if (PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D))
                {
                    // 3D레이로 밑에 무언가 있다면 이동 코루틴 종료
                    if (GameLibrary.Raycast3D(m_rigidbody.transform.position, Vector3.down, stopDistanceY, layerMask))
                        break;
                }
                // 2D일 경우
                else
                {
                    // 2D레이로 밑에 무언가 있다면 이동 코루틴 종료
                    if (GameLibrary.Raycast2D(m_rigidbody.transform.position, Vector2.down, stopDistanceY, layerMask))
                        break;
                }
            }
            else
            {
                m_rigidbody.velocity = Vector3.zero;
            }

            yield return null;
        }

        m_rigidbody.isKinematic = true;
    }

    /// <summary>폭탄 로직</summary>
    private IEnumerator BombLogic()
    {
        m_meshRenderer.material = GameLibrary.Material_Red;

        yield return new WaitForSeconds(m_bombTimer);

        EffectManager.Instance.CreateEffect(Effect_Type.Enemy_Boom, m_rigidbody.transform.position);

        m_meshRenderer.material = m_startMaterial;
    }
}
