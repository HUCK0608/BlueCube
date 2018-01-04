using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    // 총알 매니저
    private Bullets m_manager;

    // 총알이 사용중인지 여부
    private bool m_isUsed;
    public bool IsUsed { get { return m_isUsed; } }

    private void Awake()
    {
        m_manager = transform.GetComponentInParent<Bullets>();
    }

    // 총알 발사
    public void Shoot(Vector3 start, Vector3 direction)
    {
        m_isUsed = true;

        // 시작위치로 이동
        transform.position = start;

        // 이동 코루틴 시작
        StartCoroutine(Move(direction));
    }

    // 총알 이동 코루틴
    private IEnumerator Move(Vector3 direction)
    {
        // 정면으로 계속 이동
        while(IsUsed)
        {
            transform.Translate(direction * m_manager.Stat.Speed * Time.deltaTime);
            yield return null;
        }
    }
}
