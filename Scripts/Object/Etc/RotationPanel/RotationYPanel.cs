using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RotationYPanel : MonoBehaviour
{
    [SerializeField]
    private float m_rotationSpeed;

    [SerializeField]
    private E_RotationDir m_rotationDirection;

    private Vector3 m_rotation;

    private Transform m_modelAndCollider3D;
    private BoxCollider2D m_collider2D;

    private void Awake()
    {
        m_rotation = Vector3.zero;

        m_rotation.y = m_rotationSpeed * (int)m_rotationDirection;

        m_modelAndCollider3D = transform.Find("ModelAndCollider3D");
        m_collider2D = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        if (GameLibrary.Bool_IsCOV2D)
            return;

        PanelRotate();
        SetCollider2DSize();
    }

    private void PanelRotate()
    {
        m_modelAndCollider3D.eulerAngles += m_rotation * Time.deltaTime;
    }

    // 2D 콜라이더 사이즈 계산
    private void SetCollider2DSize()
    {
        float temp = 45f;
        float defaultSizeX = 8f;

        float anglyY = m_modelAndCollider3D.eulerAngles.y;
        // y각도를 90으로 나눈 나머지로 함
        float calcY = anglyY % 90f;
        // 최대 사이즈가 11이니 최대사이즈에서 최소사이즈를 뺀 3으로 3f / 45f를 해줌
        float increaseSizePer = 3f / temp;

        // 90으로 나눈 나머지가 45보다 클 경우
        if (calcY >= temp)
            // 45에서 다시 점점 작아지게 하기 위해 45 - (90으로 나눈 나머지를 45로 나눔)
            calcY = temp - (calcY % temp);

        // 새로운 2D 콜라이더 크기를 구함
        Vector2 newSize = m_collider2D.size;
        // 제일 작은 사이즈인 8 + (퍼센트 * 계산된y), 퍼센트 * 계산된y는 최대 3까지 늘어남
        newSize.x = defaultSizeX + (increaseSizePer * calcY);

        // 사이즈 적용
        m_collider2D.size = newSize;
    }
}
