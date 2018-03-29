using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChangeViewRect : MonoBehaviour
{
    // 블루큐브
    private Transform m_blueCube;

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

    private void Awake()
    {
        m_blueCube = GameManager.Instance.BlueCubeManager.transform;

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
        // 상자 비활성화
        gameObject.SetActive(false);
    }
    
    /// <summary>상자의 x, y 크기가 커짐. 최대 크기까지 커질 경우 종료</summary>
    public IEnumerator IncreaseSizeXY()
    {
        transform.localScale = Vector3.zero;
        transform.position = m_blueCube.position;
        // 상자의 충돌체크 켜기
        SetColliderEnable(true);
        // 상자 활성화
        gameObject.SetActive(true);

        while(true)
        {
            // 상자의 x, y 크기를 키움
            transform.localScale += m_increaseSizeValueXY * Time.deltaTime;

            // 최대 크기만큼 커졌을 경우 반복문 종료
            if (transform.localScale.x >= m_increaseMaxSize.x)
                break;

            yield return null;
        }
    }

    public void SetSizeZ(float positionZ)
    {

    }

    ///<summary>상자 충돌체크 설정</summary>
    public void SetColliderEnable(bool value)
    {
        m_collider.enabled = value;
    }
}
