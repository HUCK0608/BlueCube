using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Key : MonoBehaviour
{
    // 메쉬
    private MeshRenderer m_meshRenderer;

    // 콜라이더
    private Collider m_collider;
    private Collider2D m_collider2D;

    /// <summary>연결된 문</summary>
    private Door_Key m_connectDoor;
    /// <summary>연결된 문 설정</summary>
    public void SetConnectDoor(Door_Key newDoor) { m_connectDoor = newDoor; }

    /// <summary>이동속도</summary>
    [SerializeField]
    private float m_moveSpeed = 10f;

    /// <summary>기본 이펙트</summary>
    [SerializeField]
    private GameObject m_defaultEffect;

    private void Awake()
    {
        m_meshRenderer = GetComponentInChildren<MeshRenderer>();
        m_collider = GetComponentInChildren<Collider>();
        m_collider2D = GetComponentInChildren<Collider2D>();

        StartCoroutine(EffectControll());
    }

    /// <summary>시점에 따른 이펙트 조절</summary>
    private IEnumerator EffectControll()
    {
        WaitUntil onMeshRendererWaitUntil = new WaitUntil(() => m_meshRenderer.enabled);
        WaitUntil offMeshRendererWaitUntil = new WaitUntil(() => !m_meshRenderer.enabled);

        while (true)
        {
            yield return offMeshRendererWaitUntil;
            m_defaultEffect.SetActive(false);
            yield return onMeshRendererWaitUntil;
            m_defaultEffect.SetActive(true);
        }
    }

    /// <summary>착지지점으로 날아가기 시작</summary>
    public void StartFlyToLandingPosition()
    {
        m_collider.enabled = false;
        m_collider2D.enabled = false;
        m_defaultEffect.SetActive(false);

        StartCoroutine(FlyToLandingPositionLogic());
    }

    /// <summary>착지 지점으로 날아가는 로직</summary>
    private IEnumerator FlyToLandingPositionLogic()
    {
        Vector3 landingPosition = m_connectDoor.GetKeyLandingPosition();

        // 안착 지점으로 날라가는 로직
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, landingPosition, m_moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, landingPosition) <= 0.01f)
                break;

            yield return null;
        }

        EffectManager.Instance.CreateEffect(Effect_Type.Key_Landing, transform.position);
        m_connectDoor.CompleteLanding();
        gameObject.SetActive(false);
    }
}
