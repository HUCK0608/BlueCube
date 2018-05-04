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

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_stat = GetComponent<EnemyStat_Boomer>();
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
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                // 플레이어가 탐지 될 경우 반복문을 종료하고 폭탄 발사 코루틴 실행
                if (m_detectionArea.CheckDetected(PlayerManager.Instance.Player3D_Object.transform.position))
                    break;
            }
            yield return null;
        }

        StartCoroutine(ShootBomb());
    }

    // 폭탄 발사
    private IEnumerator ShootBomb()
    {
        Transform player = PlayerManager.Instance.Player3D_Object.transform;

        float addTime = 0f;

        bool isInit = true;
        float startShootRandomTime = Random.Range(m_stat.StartShootMinDelay, m_stat.StartShootMaxDelay);

        while(true)
        {
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                RotateToPlayer();

                addTime += Time.deltaTime;

                if (addTime >= m_stat.ShootDelay && !isInit)
                {
                    EnemyProjectileManager.Instance.UseProjectile(E_EnemyProjectile.Bomb, m_stat.ShootPosition.position, player.position);
                    addTime = 0f;
                }
                else if(isInit && addTime >= startShootRandomTime)
                {
                    EnemyProjectileManager.Instance.UseProjectile(E_EnemyProjectile.Bomb, m_stat.ShootPosition.position, player.position);
                    addTime = 0f;
                    isInit = false;
                }

                if (!m_detectionArea.CheckDetected(player.position))
                    break;
            }
            yield return null;
        }

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
