using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PE_DamageKnockBack : MonoBehaviour
{
    [SerializeField]
    private float m_knockBackPower_XZ;
    /// <summary>X, Z 넉백파워</summary>
    public float KnockBackPower_XZ { get { return m_knockBackPower_XZ; } }

    [SerializeField]
    private float m_knockBackPower_Y;
    /// <summary>Y 넉백파워</summary>
    public float KnockBackPower_Y { get { return m_knockBackPower_Y; } }

    [SerializeField]
    private float m_damage;
    /// <summary>데미지</summary>
    public float Damage { get { return m_damage; } }

    // 히트 허용 시간
    [SerializeField]
    private float m_hitActiveTime;
    // 히트 허용 변수
    private bool m_hitActive;

    // 한번의 충돌체크를 하기 위한 히트 리스트
    private List<GameObject> m_hitList;

    private void Awake()
    {
        m_hitList = new List<GameObject>();
    }

    private void Start()
    {
        // 데미지 체크 코루틴 시작
        StartCoroutine(DamageCheckTimer());
    }

    // 언제까지 데미지를 입힐지 체크하는 타이머
    private IEnumerator DamageCheckTimer()
    {
        float addTime = 0f;
        // 히트 활성화
        m_hitActive = true;

        while(true)
        {
            // 시점변환 중이 아니고 2D시점이 아닐경우에만 타이머 체크
            if (!GameManager.Instance.PlayerManager.Skill_CV.IsChanging && !GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(GameLibrary.Enum_View2D))
            {
                addTime += Time.deltaTime;

                if (addTime >= m_hitActiveTime)
                    break;
            }
            yield return null;
        }

        // 히트 비활성화
        m_hitActive = false;
    }

    /// <summary>데미지를 입힌 물체인지 체크하는 함수 (hitObject가 hitList에 있으면 true 아니면 flase를 반환)</summary>
    public bool IsHit(GameObject hitObject)
    {
        // 히트 활성화가 아니면 리턴
        if (!m_hitActive)
            return true;

        int hitObjectIndex = m_hitList.IndexOf(hitObject);
        int nullIndex = -1;

        // 리스트에 포함되어 있지 않은 오브젝트이면
        if(hitObjectIndex.Equals(nullIndex))
        {
            // 리스트에 포함시키고 false를 반환
            m_hitList.Add(hitObject);
            return false;
        }

        // 리스트에 포함되어 있으면 true를 반환
        return true;
    }
}
