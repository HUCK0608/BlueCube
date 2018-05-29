using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStat_Boomer))]
public sealed class Enemy_Boomer : MonoBehaviour
{
    private WorldObject m_worldObject;

    // 탐지 공간
    [SerializeField]
    private EnemyDetectionArea m_detectionArea;

    // 스탯
    private EnemyStat_Boomer m_stat;

    /// <summary>애니메이터</summary>
    private Animator m_animator;

    /// <summary>임시 숲 스크립트</summary>
    private SoopTemp m_soopTemp;

    /// <summary>발사 준비중일경우 true를 반환 (애니메이션 변수)</summary>
    private bool m_isShootInit;

    /// <summary>발사중일경우 true를 반환 (애니메이션 변수)</summary>
    private bool m_isShoot;

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_stat = GetComponent<EnemyStat_Boomer>();

        m_animator = GetComponentInChildren<Animator>();
        m_soopTemp = GetComponentInChildren<SoopTemp>();
    }

    private void Start()
    {
        StartCoroutine(CheckDetectionPlayer());
    }

    // 플레이어 탐지 체크
    private IEnumerator CheckDetectionPlayer()
    {
        while(true)
        {
            // 플레이어가 탐지 될 경우 반복문을 종료하고 폭탄 발사 코루틴 실행
            if (m_detectionArea.CheckDetected(PlayerManager.Instance.Player3D_Object.transform.position))
                break;

            yield return null;
        }

        StartCoroutine(ShootBombLogic());
    }

    /// <summary>폭탄 발사 로직</summary>
    private IEnumerator ShootBombLogic()
    {
        Transform player = PlayerManager.Instance.Player3D_Object.transform;

        float addTime = 0f;

        bool isInit = true;
        float startShootRandomTime = Random.Range(m_stat.StartShootMinDelay, m_stat.StartShootMaxDelay);

        m_isShootInit = true;

        string isShootInitPath = "IsShootInit";
        string isShootPath = "IsShoot";

        while(true)
        {
            m_animator.SetBool(isShootInitPath, m_isShootInit);
            m_animator.SetBool(isShootPath, m_isShoot);

            RotateToPlayer();

            if(m_soopTemp.IsEndShootMotion)
            {
                m_isShoot = false;
                m_soopTemp.IsEndShootMotion = false;
            }

            if (m_soopTemp.IsEndShootInitMotion)
                addTime += Time.deltaTime;

            if (addTime >= m_stat.ShootDelay)
            {
                m_soopTemp.IsEndShootInitMotion = false;
                m_isShoot = true;

                EnemyProjectileManager.Instance.UseProjectile(E_EnemyProjectile.Bomb, m_stat.ShootPosition.position, player.position);
                addTime = 0f;
            }
            //else if(isInit && addTime >= startShootRandomTime)
            //{
            //    EnemyProjectileManager.Instance.UseProjectile(E_EnemyProjectile.Bomb, m_stat.ShootPosition.position, player.position);
            //    addTime = 0f;
            //    isInit = false;
            //}

            if (!m_detectionArea.CheckDetected(player.position))
                break;

            yield return null;
        }

        m_isShootInit = false;
        m_isShoot = false;
        m_soopTemp.IsEndShootInitMotion = false;
        m_soopTemp.IsEndShootMotion = false;

        m_animator.SetBool(isShootInitPath, m_isShootInit);
        m_animator.SetBool(isShootPath, m_isShoot);

        // 플레이어 탐지 코루틴 실행
        StartCoroutine(CheckDetectionPlayer());
    }

    // 플레이어를 향해 회전
    private void RotateToPlayer()
    {
        Vector3 playerPosition = PlayerManager.Instance.Player3D_Object.transform.position;
        playerPosition.y = transform.position.y;

        Vector3 directionToPlayer = playerPosition - transform.position;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), m_stat.RotationSpeed * Time.deltaTime);
    }
}
