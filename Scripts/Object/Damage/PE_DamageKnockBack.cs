using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PE_DamageKnockBack : MonoBehaviour
{
    [SerializeField]
    private float m_knockBackPower;
    public float KnockBackPower { get { return m_knockBackPower; } }

    [SerializeField]
    private float m_damage;
    public float Damage { get { return m_damage; } }
}
