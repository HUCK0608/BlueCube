using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChangeViewRect : MonoBehaviour
{
    // 스킬
    private PlayerSkill_ChangeView m_skill;

    // 2D 외벽 콜라이더
    private GameObject m_outWallGroup;

    [SerializeField]
    private GameObject m_changeViewEffect;

    // 콜라이더
    private Collider m_collider;

    // 최대로 증가할 수 있는 크기
    [SerializeField]
    private Vector3 m_increaseMaxSize;

    // 프레임당 상자 x, y 크기가 몇 퍼센트씩 커지게 할건지
    [SerializeField]
    private float m_increaseSizePerXY;

    // 프레임당 상자가 커지는 실제 수치
    private Vector3 m_increaseSizeValueXY;

    // 상자 속성 설정 부분
    ///<summary>상자 충돌체크 설정</summary>
    public void SetColliderEnable(bool value) { m_collider.enabled = value; }
    /// <summary>활성화 여부</summary>
    public void SetActive(bool value) { gameObject.SetActive(value); }

    /// <summary>상자의 z크기를 반환</summary>
    public float SizeZ { get { return transform.localScale.z; } }

    private void Awake()
    {
        m_skill = GetComponentInParent<PlayerSkill_ChangeView>();

        m_outWallGroup = transform.parent.Find("OutWallGroup").gameObject;

        m_collider = GetComponent<Collider>();

        InitChangeViewRect();
    }
    
    // 상자에 필요한 변수 초기화 및 계산
    private void InitChangeViewRect()
    {
        // x, y 프레임당 증가수치를 구한다.
        // 총 증가수치 * 퍼센트
        float increaseSizeValueX = m_increaseMaxSize.x * m_increaseSizePerXY * 0.01f;
        float increaseSizeValueY = m_increaseMaxSize.y * m_increaseSizePerXY * 0.01f;

        m_increaseSizeValueXY = new Vector3(increaseSizeValueX, increaseSizeValueY, 0f);

        // 상자의 충돌체크 끄기
        SetColliderEnable(false);
        // 외벽 비활성화
        m_outWallGroup.SetActive(false);
    }

    /// <summary>2D상태로 시작</summary>
    public void StartView2D()
    {
        m_outWallGroup.transform.localScale = m_increaseMaxSize;
        m_outWallGroup.SetActive(true);
    }

    /// <summary>상자의 x, y 크기가 커짐. 최대 크기까지 커질 경우 종료</summary>
    public IEnumerator SetIncreaseSizeXY()
    {
        // 이펙트 켜기
        m_changeViewEffect.SetActive(true);

        Vector3 startScale = Vector3.zero;
        startScale.z = 0.01f;

        Vector3 player3DPosition = PlayerManager.Instance.Player3D_Object.transform.position;

        transform.localScale = startScale;
        transform.position = player3DPosition;

        // 상자의 충돌체크 켜기
        SetColliderEnable(true);
        // 상자 활성화
        gameObject.SetActive(true);

        while(true)
        {
            if (!UIManager.Instance.IsOnTabUI)
            {
                // 상자의 x, y 크기를 키움
                transform.localScale += m_increaseSizeValueXY * Time.deltaTime;

                // 최대 크기만큼 커졌을 경우 반복문 종료
                if (transform.localScale.x >= m_increaseMaxSize.x)
                    break;
            }
            yield return null;
        }
    }

    /// <summary>마우스 좌표로 상자의 z의 크기가 커짐</summary>
    public IEnumerator SetSizeZToMousePoint()
    {
        Vector3 player3DPosition = PlayerManager.Instance.Player3D_Object.transform.position;

        Vector3 newRectSize = transform.localScale;

        // lerp 수치
        float lerpT = 0.1f;

        Vector3 startPosition = transform.position;

        while(true)
        {

            Vector3 hitPoint = CameraManager.Instance.GetMouseHitPointToPivot(player3DPosition);

            // 충돌된 z좌표를 가져와서 새로운 크기 계산을 함
            float hitPositionZ = hitPoint.z;
            newRectSize.z = hitPositionZ - startPosition.z;
            newRectSize.z = Mathf.Clamp(newRectSize.z, 0.1f, m_increaseMaxSize.z);

            transform.localScale = Vector3.Lerp(transform.localScale, newRectSize, lerpT);

            // 계산된 z 좌표를 가져옴
            Vector3 newPosition = transform.position;
            newPosition.z = player3DPosition.z + transform.localScale.z * 0.5f;

            // 이동
            transform.position = newPosition;

            if (m_skill.IsDoChange || m_skill.IsNotChange)
                break;

            yield return null;
        }

        m_changeViewEffect.SetActive(false);

        // 충돌체크 끄기
        SetColliderEnable(false);
    }

    /// <summary>상자 작아지게 만들기</summary>
    public IEnumerator SetDecreaseSize()
    {
        // 이펙트가 켜져있다면 이펙트를 끔
        if (m_changeViewEffect.activeSelf)
            m_changeViewEffect.SetActive(false);

        // 활성화가 되어있지 않을경우 활성화
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        Vector3 player3DPosition = PlayerManager.Instance.Player3D_Object.transform.position;

        // 감소수치 계산
        Vector3 decreaseValue = -(transform.localScale * m_increaseSizePerXY * 0.01f);

        // 기존 x좌표 및 x크기
        float oldPositonX = transform.position.x;
        float oldScaleX = transform.localScale.x;

        while (true)
        {
            // 크기 줄이기
            transform.localScale += decreaseValue * Time.deltaTime;

            // 이동좌표 가져오기
            Vector3 newPosition = transform.position;
            newPosition.z = player3DPosition.z + transform.localScale.z * 0.5f;

            // 이동
            transform.localPosition = newPosition;

            // 최소수치만큼 작아졌을 경우 반복문 종료
            if (transform.localScale.x <= 0f)
                break;

            yield return null;
        }

        // 비활성화
        gameObject.SetActive(false);
    }

    /// <summary>외벽 활성화 여부</summary>
    public void SetOutWallEnable(bool value)
    {
        if (value)
            m_outWallGroup.transform.localScale = transform.localScale;
        m_outWallGroup.transform.position = transform.position;
        m_outWallGroup.SetActive(value);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 아닐경우
        if(!other.tag.Equals(GameLibrary.String_Player))
        {
            // 월드오브젝트 관련 스크립트 가져오기
            WorldObject worldObject = other.GetComponentInParent<WorldObject>();

            // 스크립트가 있을 경우에 메테리얼을 변경
            if (worldObject != null)
            {
                worldObject.SetMaterial(E_WorldObject_ShaderType.CanChange);
                // 상자에 포함되어있다고 알리기
                worldObject.IsIncludeChangeViewRect = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 아닐경우
        if(!other.tag.Equals(GameLibrary.String_Player))
        {
            // 월드오브젝트 관련 스크립트 가져오기
            WorldObject worldObject = other.GetComponentInParent<WorldObject>();

            // 스크립트가 있을 경우에 메테리얼을 변경
            if (worldObject != null)
            {
                worldObject.SetMaterial(E_WorldObject_ShaderType.Default3D);
                // 상자에 포함되지않았다고 알리기
                worldObject.IsIncludeChangeViewRect = false;
            }
        }
    }
}
