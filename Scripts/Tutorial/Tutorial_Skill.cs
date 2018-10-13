using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tutorial_Skill : Tutorial
{
    [Header("Mouse")]
    /// <summary>마우스 이미지</summary>
    [SerializeField]
    private Image m_mouseImage;

    /// <summary>마우스 피벗</summary>
    [SerializeField]
    private Transform m_mousePivot;

    /// <summary>마우스 스프라이트(0 : 디폴트, 1 : 클릭)</summary>
    [SerializeField]
    private Sprite[] m_mouseSprites;

    /// <summary>캔버스</summary>
    private Canvas m_canvas;

    [Header("Arrow")]
    /// <summary>화살표 머리</summary>
    [SerializeField]
    private Transform m_arrowHead;

    /// <summary>화살표 몸 피벗</summary>
    [SerializeField]
    private Transform m_arrowBodyPivot;

    /// <summary>화살표 머리 피벗</summary>
    [SerializeField]
    private Transform m_arrowHeadPivot;

    /// <summary>화살표 몸이 최대로 커지는 크기</summary>
    [SerializeField]
    private float m_arrowBodyMaxSize;

    /// <summary>화살표가 늘어나는 시간</summary>
    [SerializeField]
    private float m_arrowIncreaseSizeTime;

    private Coroutine m_logicCor, m_calcMousePositionCor;

    private void Start()
    {
        m_canvas = GameObject.Find("UI").GetComponent<Canvas>();

        gameObject.SetActive(false);
    }

    public override void StartTutorial()
    {
        // 스킬 잠금 해제
        PlayerManager.Instance.Skill.SetSkillLock(false);

        gameObject.SetActive(true);

        m_logicCor = StartCoroutine(TutorialLogic());
        m_calcMousePositionCor = StartCoroutine(CalcMousePoistion());
        StartCoroutine(EndCheck());
    }

    private IEnumerator TutorialLogic()
    {
        float increaseValue = m_arrowBodyMaxSize / m_arrowIncreaseSizeTime;

        Vector3 arrowBodyInitScale = new Vector3(1f, 1f, 0f);
        Vector3 headPoistionError = new Vector3(0f, 0f, -19f);

        // 마우스 이미지 교체시 딜레이
        WaitForSeconds delay1 = new WaitForSeconds(0.5f);

        while (true)
        {
            m_arrowBodyPivot.localScale = arrowBodyInitScale;
            SetMouseSpirte(m_mouseSprites[0]);

            m_arrowHead.gameObject.SetActive(false);

            yield return delay1;

            SetMouseSpirte(m_mouseSprites[1]);

            yield return delay1;

            m_arrowHead.gameObject.SetActive(true);

            while (true)
            {
                Vector3 arrowBodyScale = m_arrowBodyPivot.localScale;

                if (arrowBodyScale.z >= (m_arrowBodyMaxSize))
                {
                    arrowBodyScale.z = 0f;
                    break;
                }

                arrowBodyScale.z += increaseValue * Time.deltaTime;

                m_arrowBodyPivot.localScale = arrowBodyScale;
                m_arrowHead.position = m_arrowHeadPivot.position + headPoistionError;

                yield return null;
            }


            yield return delay1;

            SetMouseSpirte(m_mouseSprites[0]);

            yield return delay1;
        }
    }

    /// <summary>마우스 위치 계산 코루틴</summary>
    private IEnumerator CalcMousePoistion()
    {
        while(true)
        {
            Vector3 mousePivotScreenPosition = Camera.main.WorldToViewportPoint(m_mousePivot.position);

            mousePivotScreenPosition.x *= m_canvas.pixelRect.width;
            mousePivotScreenPosition.y *= m_canvas.pixelRect.height;

            m_mouseImage.rectTransform.position = mousePivotScreenPosition;

            yield return null;
        }
    }

    /// <summary>마우스 스프라이트 교체</summary>
    private void SetMouseSpirte(Sprite sprite)
    {
        m_mouseImage.sprite = sprite;
    }

    private IEnumerator EndCheck()
    {
        yield return new WaitUntil(() => PlayerManager.Instance.IsViewChange);

        EndTutorial();
    }

    protected override void EndTutorial()
    {
        StopCoroutine(m_logicCor);
        StopCoroutine(m_calcMousePositionCor);
        gameObject.SetActive(false);
    }
}
