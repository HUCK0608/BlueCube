using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_MaterialType { Default, Change, Block }

[SelectionBase]
public class WorldObject : MonoBehaviour
{
    protected static string Shader_ChoiceString = "_Choice";

    // 2D 텍스쳐를 사용할지 여부
    [SerializeField]
    protected bool m_isUse2DTexture;

    protected bool m_isOnRenderer;
    /// <summary>오브젝트의 렌더러가 활성화 되어 있을 경우 true를 반환</summary>
    public bool IsOnRenderer { get { return m_isOnRenderer; } }

    protected bool m_isIncludeChangeViewRect;
    /// <summary>시점변환 상자에 포함되어 있으면 true를 반환</summary>
    public bool isIncludeChangeViewRect { get { return m_isIncludeChangeViewRect; } set { m_isIncludeChangeViewRect = value; } }

    /// <summary>끼임 오브젝트를 표시하는 중이면 true를 반환</summary>
    protected bool m_isShowBlock;

    protected virtual void Awake()
    {
        m_isOnRenderer = true;
    }

    /// <summary>오브젝트를 2D상태로 변경</summary>
    public virtual void Change2D() { }
    /// <summary>오브젝트를 3D상태로 변경</summary>
    public virtual void Change3D() { }
    /// <summary>오브젝트의 렌더러 활성화 여부를 설정</summary>
    public virtual void SetRendererEnable(bool value) { }
    /// <summary>오브젝트의 메테리얼을 변경</summary>
    public virtual void SetMaterial(E_MaterialType materialType) { }

    /// <summary>끼인 오브젝트가 무엇인지 보여준다</summary>
    public void ShowBlock()
    {
        // 오브젝트를 보여주는 중이 아닐경우 로직 실행
        if(!m_isShowBlock)
            StartCoroutine(ShowBlockLogic());
    }

    /// <summary>끼인 오브젝트 로직 코루틴</summary>
    private IEnumerator ShowBlockLogic()
    {
        m_isShowBlock = true;

        // 현재 반복회수 및 누적시간
        int currentCycle = 0;
        float addTime = 0f;

        // Block과 Change 머테리얼을 왔다 갔다 하기 위한 변수
        bool isShowBlock = true;

        // 시점변환 준비중이고 시점변환 상자에 포함되어 있을 경우에만 실행
        while(PlayerManager.Instance.IsViewChangeReady && isIncludeChangeViewRect)
        {
            // 시간 누적
            addTime += Time.deltaTime;

            // 누적된 시간이 반복 시간만큼 됬을 경우
            if(addTime >= WorldManager.Instance.ShowBlockCycleTime)
            {
                // 상태에 따라 메테리얼 변경
                if(isShowBlock)
                {
                    SetMaterial(E_MaterialType.Block);
                    isShowBlock = false;
                }
                else
                {
                    SetMaterial(E_MaterialType.Change);
                    isShowBlock = true;

                    currentCycle++;
                }

                // 모든 사이클을 돌았을 경우 코루틴 종료
                if (currentCycle.Equals(WorldManager.Instance.MaxShowBlockCycleCount))
                    break;

                // 누적시간 초기화
                addTime = 0f;
            }

            yield return null;
        }

        // 현재 오브젝트 상태에 따라 메테리얼 재설정
        if (isIncludeChangeViewRect)
            SetMaterial(E_MaterialType.Change);
        else
            SetMaterial(E_MaterialType.Default);

        m_isShowBlock = false;
    }
}
