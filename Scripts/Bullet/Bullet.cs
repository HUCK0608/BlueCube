using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BulletOwner { Player, Enemy }

public sealed class Bullet : MonoBehaviour
{
    // 총알 묶음 스크립트
    private BulletBundle m_bundle;
    public BulletBundle Bundle { get { return m_bundle; } }

    // 총알이 사용중인지 여부
    private bool m_isUsed;
    public bool IsUsed { get { return m_isUsed; } }

    private bool m_isShoot;

    // 총알의 소유자
    [SerializeField]
    private E_BulletOwner m_bulletOwner;

    // 충돌 이펙트 타입
    [SerializeField]
    Effect_Type m_colEffectType;

    private WorldObject m_worldObject;

    private void Awake()
    {
        m_bundle = transform.GetComponentInParent<BulletBundle>();

        m_worldObject = GetComponent<WorldObject>();
    }

    // 총알 발사
    public void Shoot(Vector3 start, Vector3 direction)
    {
        m_isUsed = true;

        // 시작위치로 이동
        transform.position = start;

        // 회전
        transform.rotation = Quaternion.LookRotation(direction);

        // 이동 코루틴 시작
        StartCoroutine(Move(direction));
    }

    // 총알 이동 코루틴
    private IEnumerator Move(Vector3 direction)
    {
        // 누적시간
        float accTime = 0;

        m_isShoot = true;

        // 정면으로 계속 이동
        while(m_isShoot)
        {
            // 소유자가 적일경우 2D가 아닐때만 이동
            if (!(m_bulletOwner.Equals(E_BulletOwner.Enemy) && GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(GameLibrary.Enum_View2D)))
            {
                // 시점변환중이 아니고 활성화 상태일경우만 이동
                if (!GameManager.Instance.PlayerManager.Skill_CV.IsChanging && m_worldObject.Enabled)
                {
                    transform.Translate(direction * m_bundle.Stat.Speed, Space.World);

                    // 시간 누적
                    accTime += Time.fixedDeltaTime;

                    // 지속시간이 지났을 경우 발사 정지
                    if (accTime >= m_bundle.Stat.DurationTime)
                    {
                        EndShoot();
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }

        // 이펙트 생성
        GameManager.Instance.EffectManager.CreateEffect(m_colEffectType, transform.position);

        // 먼 지점으로 날림
        transform.localPosition = Vector3.zero;

        // 렌더링 및 2D 콜라이더 비활성화
        m_worldObject.RendererEnable(false);
        m_worldObject.Collider2DEnable(false);

        m_isUsed = false;
    }

    // 총알발사를 멈춤
    public void EndShoot()
    {
        m_isShoot = false;
    }
}
