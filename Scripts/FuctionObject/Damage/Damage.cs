using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private E_HitType m_hitType;
    public E_HitType HitType { get { return m_hitType; } }

    // 피격을 입힌 리스트
    protected List<GameObject> m_hitList;

    private void Awake()
    {
        m_hitList = new List<GameObject>();
    }

    /// <summary>collisionTag를 비교하여 피격을 입힌다.</summary>
    public virtual void DoHit(GameObject collision) { }
}
