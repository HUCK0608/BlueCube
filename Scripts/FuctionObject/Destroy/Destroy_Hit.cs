using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_HitType { Boom }
public sealed class Destroy_Hit : MonoBehaviour
{
    // 이 오브젝트가 부셔질 히트 타입
    [SerializeField]
    private E_HitType m_destroyHitType;

    /// <summary>hitType과 부셔질 피격타입을 비교하여 같을경우 오브젝트가 부셔짐</summary>
    public void Destroy(E_HitType hitType)
    {
        // 받은 피격타입과 부셔질 피격타입이 같으면 오브젝트를 부심
        if(hitType.Equals(m_destroyHitType))
        {
            gameObject.SetActive(false);
        }
    }
}
