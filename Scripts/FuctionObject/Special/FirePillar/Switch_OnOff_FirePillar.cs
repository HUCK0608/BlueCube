using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_OnOff_FirePillar : Switch_OnOff
{
    private Interaction_Push_FirePillar m_firePillar;
    private Special_FirePillar_Fire m_fire;

    protected override void Awake()
    {
        base.Awake();

        m_firePillar = GetComponent<Interaction_Push_FirePillar>();
        m_fire = GetComponent<Special_FirePillar_Fire>();

        SwitchOn();
    }

    protected override IEnumerator SwitchOnLogic()
    {
        yield return new WaitUntil(() => m_firePillar.FirePillarColorType.Equals(m_fire.CurrentFireColorType));
        m_isOn = true;

        StartCoroutine(SwitchOffLogic());
    }

    protected override IEnumerator SwitchOffLogic()
    {
        yield return new WaitUntil(() => !m_firePillar.FirePillarColorType.Equals(m_fire.CurrentFireColorType));
        m_isOn = false;

        StartCoroutine(SwitchOnLogic());
    }
}
