using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_WorldObject_ShaderType { Default3D, Default2D, CanChange, Block, BackGround }

[SelectionBase]
public class WorldObject : MonoBehaviour
{
    protected static string m_shader_ChoiceString = "_Choice";

    // 2D 텍스쳐를 사용할지 여부
    [SerializeField]
    protected bool m_isUse2DTexture;
    // 샤이닝 스펙트럼을 사용할지 여부
    [SerializeField]
    protected bool m_isUseShiningSpecular;

    protected bool m_isOnRenderer;
    /// <summary>오브젝트의 렌더러가 활성화 되어 있을 경우 true를 반환</summary>
    public bool IsOnRenderer { get { return m_isOnRenderer; } }

    protected bool m_isIncludeChangeViewRect;
    /// <summary>시점변환 상자에 포함되어 있으면 true를 반환</summary>
    public bool IsIncludeChangeViewRect { get { return m_isIncludeChangeViewRect; } set { m_isIncludeChangeViewRect = value; } }

    protected bool m_isIncludeChangeViewRectZ;
    public bool IsIncludeChangeVeiwRectZ { get { return m_isIncludeChangeViewRectZ; } set { m_isIncludeChangeViewRectZ = value; } }

    /// <summary>끼임 오브젝트를 표시하는 중이면 true를 반환</summary>
    protected bool m_isShowBlock;

    protected virtual void Awake()
    {
        m_isOnRenderer = true;
    }

    /// <summary>2D 상태로 시작</summary>
    public void StartView2D()
    {
        m_isIncludeChangeViewRect = true;
        Change2D();
    }

    /// <summary>오브젝트를 2D상태로 변경</summary>
    public virtual void Change2D() { }
    /// <summary>오브젝트를 3D상태로 변경</summary>
    public virtual void Change3D() { }
    /// <summary>오브젝트의 렌더러 활성화 여부를 설정</summary>
    public virtual void SetRendererEnable(bool value) { }
    /// <summary>오브젝트의 쉐이더를 변경</summary>
    public virtual void SetMaterial(E_WorldObject_ShaderType ShaderType) { }

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
        while(PlayerManager.Instance.IsViewChangeReady && IsIncludeChangeViewRect)
        {
            // 시간 누적
            addTime += Time.deltaTime;

            // 누적된 시간이 반복 시간만큼 됬을 경우
            if(addTime >= WorldManager.Instance.ShowBlockCycleTime)
            {
                // 상태에 따라 메테리얼 변경
                if(isShowBlock)
                {
                    SetMaterial(E_WorldObject_ShaderType.Block);
                    isShowBlock = false;
                }
                else
                {
                    SetMaterial(E_WorldObject_ShaderType.Default3D);
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
        if (IsIncludeChangeViewRect)
            SetMaterial(E_WorldObject_ShaderType.CanChange);
        else
            SetMaterial(E_WorldObject_ShaderType.Default3D);

        m_isShowBlock = false;
    }
}
