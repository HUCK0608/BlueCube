using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Damage : MonoBehaviour
{
    [SerializeField]
    private E_HitType m_hitType;
    public E_HitType HitType { get { return m_hitType; } }
}
