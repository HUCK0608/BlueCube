using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_HpPostion : MonoBehaviour
{
    private Transform m_model;

    // 체력증가량
    [SerializeField]
    private int m_hpIncreaseAmount;

    [SerializeField]
    private float m_upDownRange;

    [SerializeField]
    private float m_upDownMoveSpeed;

    private bool m_isUp;

    private void Awake()
    {
        m_model = transform.Find("ModelAndCollider3D");

        StartCoroutine(UpDownMove());
    }

    /// <summary>플레이어 체력 증가시키기</summary>
    public void PlayerHpIncrease()
    {
        PlayerManager.Instance.Stat.IncreaseHp(m_hpIncreaseAmount);
        gameObject.SetActive(false);
    }

    private IEnumerator UpDownMove()
    {
        Vector3 oldPosition = m_model.position;
        Vector3 upPosition = oldPosition;
        upPosition.y += m_upDownRange;
        Vector3 downPosition = oldPosition;
        downPosition.y -= m_upDownRange;

        while(true)
        {
            if (m_isUp)
                m_model.position = Vector3.MoveTowards(m_model.position, upPosition, m_upDownMoveSpeed * Time.deltaTime);
            else
                m_model.position = Vector3.MoveTowards(m_model.position, downPosition, m_upDownMoveSpeed * Time.deltaTime);

            if (m_isUp && m_model.position.Equals(upPosition))
                m_isUp = false;
            else if (!m_isUp && m_model.position.Equals(downPosition))
                m_isUp = true;

            yield return null;
        }
    }
}
