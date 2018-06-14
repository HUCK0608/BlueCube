using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Ladder : MonoBehaviour
{
    [SerializeField]
    private Transform m_hangPosition;

    [SerializeField]
    private Transform m_upPosition;

    /// <summary>사다리의 매달릴 좌표를 반환함</summary>
    public Vector3 HangPosition { get { return m_hangPosition.position; } }

    /// <summary>사다리의 위 좌표를 반환함</summary>
    public Vector3 UpPosition { get { return m_upPosition.position; } }

    /// <summary>사다리의 정면 방향을 반환함</summary>
    public Vector3 Forward { get { return transform.forward; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    /// <summary>사다리를 사용할 수 있을경우 true를 반환</summary>
    public bool IsCanUseLadder()
    {
        float PdotL = Vector3.Dot(PlayerManager.Instance.Player3D_Object.transform.forward, transform.forward);

        if (PdotL >= 0.7f)
            return true;

        return false;
    }
}
