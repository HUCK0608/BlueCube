using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Push_FirePillar : Interaction_Push
{
    [Header("[FirePillar Settings]")]
    [Space(-5f)]
    [Header("- Can Change")]
    [SerializeField]

    private E_FirePilalr_ColorType m_firePillarColorType;
    /// <summary>현재 불기둥의 색 타입을 반환</summary>
    public E_FirePilalr_ColorType FirePillarColorType { get { return m_firePillarColorType; } }
}
