using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Effect_Type { Enemy_Boom, Player_Boom, Player_ChangeView_Capsule, Player_Heal, Player_Key, Enemy_Soop_Shoot, Enemy_ChangeView }

public sealed class EffectManager : MonoBehaviour
{
    private static EffectManager m_instance;
    public static EffectManager Instance { get { return m_instance; } }

    private static string m_effectName = "Effect_";

    private Dictionary<Effect_Type, GameObject> m_effects;

    private void Awake()
    {
        m_instance = this;

        InitEffects();
    }

    private void InitEffects()
    {
        m_effects = new Dictionary<Effect_Type, GameObject>();

        Effect_Type[] effectTypes = (Effect_Type[])System.Enum.GetValues(typeof(Effect_Type));

        for (int i = 0; i < effectTypes.Length; i++)
        {
            GameObject effect = Resources.Load("Prefabs/Effect/" + m_effectName + effectTypes[i].ToString("G")) as GameObject;
            m_effects.Add(effectTypes[i], effect);
        }
    }

    public void CreateEffect(Effect_Type effectType, Vector3 position)
    {
        GameObject effect = MonoBehaviour.Instantiate(m_effects[effectType] as GameObject);

        effect.transform.parent = transform;
        effect.transform.position = position;
    }
}
