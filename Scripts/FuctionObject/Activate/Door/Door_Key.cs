using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Door_Key : Door
{
    private static string m_shader_patternFillPath = "_PatternFill";
    private static float[] m_shader_patternFillValue = new float[4] { 1.0f, 1.56f, 2.03f, 6.8f };
    private static float m_shader_patternFillTime = 3f;

    /// <summary>열쇠 이펙트 트랜스폼 모음</summary>
    [SerializeField]
    private Transform[] m_keyEffectTransforms;

    /// <summary>문양 메쉬 랜더러</summary>
    [SerializeField]
    private MeshRenderer m_patternMeshRenderer;

    /// <summary>문과 연결된 열쇠 모음</summary>
    [SerializeField]
    private Item_Key[] m_connectKey;

    /// <summary>문과 연결된 열쇠 개수</summary>
    private int m_connectKeyAmount;

    /// <summary>문이 가지고 있는 열쇠 개수</summary>
    private int m_haveKeyAmount;

    protected override void Start()
    {
        base.Start();

        InitKey();
    }

    /// <summary>키 관련 초기화</summary>
    private void InitKey()
    {
        if (m_connectKey != null)
            m_connectKeyAmount = m_connectKey.Length;

        m_haveKeyAmount = 4 - m_connectKeyAmount;

        if(!m_haveKeyAmount.Equals(4))
        {
            // 열쇠에 문을 연결시킴
            for(int i = 0; i < m_connectKeyAmount; i++)
                m_connectKey[i].SetConnectDoor(this);

            // 필요한 열쇠개수만큼의 이펙트를 끔
            for (int i = m_haveKeyAmount; i < 4; i++)
            {
                if (i >= m_haveKeyAmount)
                    m_keyEffectTransforms[i].gameObject.SetActive(false);
            }
        }

        SetPatternFill(m_shader_patternFillValue[m_haveKeyAmount - 1]);

        StartCoroutine(CheckOpenCondition());
    }

    /// <summary>문이 열리는 조건을 체크</summary>
    private IEnumerator CheckOpenCondition()
    {
        yield return new WaitUntil(() => m_haveKeyAmount.Equals(4));
        yield return new WaitUntil(() => GetPatternFill().Equals(m_shader_patternFillValue[3]));

        OpenDoor();
    }

    /// <summary>문양 패턴의 차오름 값 가져오기</summary>
    private float GetPatternFill()
    {
        return m_patternMeshRenderer.material.GetFloat(m_shader_patternFillPath);
    }

    /// <summary>문양 패턴의 차오름 값 설정</summary>
    private void SetPatternFill(float value)
    {
        m_patternMeshRenderer.material.SetFloat(m_shader_patternFillPath, value);
    }

    /// <summary>키의 착지 위치를 알려줌</summary>
    public Vector3 GetKeyLandingPosition()
    {
        return m_keyEffectTransforms[m_haveKeyAmount++].position;
    }

    /// <summary>열쇠가 착지했을 경우 실행</summary>
    public void CompleteLanding()
    {
        m_haveKeyAmount++;
        m_keyEffectTransforms[m_haveKeyAmount - 1].gameObject.SetActive(true);

        StartCoroutine(PatternFillIncrease(m_haveKeyAmount));
    }

    /// <summary>패턴의 차오름 증가</summary>
    private IEnumerator PatternFillIncrease(int haveKeyAmount)
    {
        if (haveKeyAmount <= 4)
        {
            yield return new WaitUntil(() => GetPatternFill().Equals(m_shader_patternFillValue[haveKeyAmount - 2]));

            float currentPatternFillValue = m_shader_patternFillValue[haveKeyAmount - 2];
            float nextPatternFillValue = m_shader_patternFillValue[haveKeyAmount - 1];
            float increaseValue = (nextPatternFillValue - currentPatternFillValue) / m_shader_patternFillTime;

            while (true)
            {
                currentPatternFillValue = Mathf.Clamp(currentPatternFillValue + increaseValue * Time.deltaTime, 0f, nextPatternFillValue);
                SetPatternFill(currentPatternFillValue);

                if (currentPatternFillValue.Equals(nextPatternFillValue))
                    break;

                yield return null;
            }
        }
    }

}
