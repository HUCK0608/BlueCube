using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player3D : Player
{
    // 애니메이터
    private Animator m_animator;

    // 리지드바디
    private Rigidbody m_rigidbody;

    // 땅 체크를 위한 변수들
    private Ray m_ray;
    private RaycastHit m_hit;
    private Vector3 m_boundsX;
    private Vector3 m_boundsZ;
    private Vector3 m_rayOriginY;
    private Vector3 m_onGroundUpPos;
    private Vector3 m_onGroundPos;
    private int m_ignoreLayerMask;
    private float m_rayDistance;

    protected override void Awake()
    {
        base.Awake();
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();

        InitCheckGround();
    }

    private void InitCheckGround()
    {
        m_ray = new Ray();

        // 레이 방향은 아래로 고정
        m_ray.direction = Vector3.down;

        // 콜라이더 박스 x, z 크기
        m_boundsX = new Vector3(GetComponent<Collider>().bounds.extents.x, 0, 0);
        m_boundsZ = new Vector3(0, 0, GetComponent<Collider>().bounds.extents.z);

        // 레이의 시작 y 위치
        m_rayOriginY = new Vector3(0, 0.1f, 0);

        // 플레이어를 올릴 땅 위 위치
        m_onGroundUpPos = new Vector3(0, 0.05f, 0);

        // 플레이어하고 총알을 무시하는 레이어마스크
        m_ignoreLayerMask = (-1) - ((1 << 8) | (1 << 11));

        // 레이 거리
        m_rayDistance = 0.16f;
    }

    private void Update()
    {
        CheckGround();
        Move();
        Jump();
        SetAni();
    }

    private void CheckGround()
    {
        // 4방향 십자 레이 체크
        // 4방향 레이를 쏴서 한곳에 무언가라도 닿으면 땅이라고 알리고 4곳 모두 아무것도 안 닿으면 땅이 아니라고 알림

        // 상
        m_ray.origin = transform.position + m_boundsZ + m_rayOriginY;

        if (Physics.Raycast(m_ray, m_rayDistance, m_ignoreLayerMask))
            Manager.IsGrounded = true;
        else
            Manager.IsGrounded = false;

        // 하
        m_ray.origin = transform.position - m_boundsZ + m_rayOriginY;

        if (Physics.Raycast(m_ray, m_rayDistance, m_ignoreLayerMask))
            Manager.IsGrounded = true;
        else if (!Manager.IsGrounded)
            Manager.IsGrounded = false;

        // 좌
        m_ray.origin = transform.position - m_boundsX + m_rayOriginY;

        if (Physics.Raycast(m_ray, m_rayDistance, m_ignoreLayerMask))
            Manager.IsGrounded = true;
        else if(!Manager.IsGrounded)
            Manager.IsGrounded = false;

        // 우
        m_ray.origin = transform.position + m_boundsX + m_rayOriginY;

        if (Physics.Raycast(m_ray, m_rayDistance, m_ignoreLayerMask))
            Manager.IsGrounded = true;
        else if(!Manager.IsGrounded)
            Manager.IsGrounded = false;
    }

    // 3D 이동
    private void Move()
    {
        // 현재 시점변환 중이라면 캐릭터 정지
        if (Manager.Skill_CV.IsChanging)
        {
            m_rigidbody.velocity = Vector3.zero;
            return;
        }

        // 키입력
        float moveX = Input.GetAxis("Horizontal") * Manager.Stat.MoveSpeed;
        float moveZ = Input.GetAxis("Vertical") * Manager.Stat.MoveSpeed;

        // 애니메이션 설정변수
        if (moveX == 0 && moveZ == 0)
            Manager.IsRunning = false;
        else
            Manager.IsRunning = true;

        // 중력을 사용중이지 않을 때
        if(!Manager.UseGravity)
        {
            // 이동키를 누를경우 중력 사용
            if (moveX != 0 || moveZ != 0)
                Manager.UseGravity = true;
        }

        // 벡터 형태로 변경
        Vector3 movement = Vector3.zero;

        // 두개의 키가 눌렸을 경우
        if (moveX != 0 && moveZ != 0)
        {
            // 상, 우
            if (moveX > 0 && moveZ > 0)
                movement = Vector3.right;
            // 하, 우
            else if (moveX > 0 && moveZ < 0)
                movement = Vector3.back;
            // 하, 좌
            else if (moveX < 0 && moveZ < 0)
                movement = Vector3.left;
            // 상, 좌
            else if (moveX < 0 && moveZ > 0)
                movement = Vector3.forward;
        }
        // 한개의 키가 눌렸을 경우
        else if (moveX != 0 || moveZ != 0)
        {
            // 상
            if (moveZ > 0)
                movement = Vector3.forward + Vector3.right;
            // 하
            else if (moveZ < 0)
                movement = Vector3.back + Vector3.left;
            // 좌
            else if (moveX < 0)
                movement = Vector3.forward + Vector3.left;
            // 우
            else if (moveX > 0)
                movement = Vector3.back + Vector3.right;
        }

        // 이동을 하는 경우에만 캐릭터 회전
        if (movement != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Manager.Stat.RotationSpeed * Time.deltaTime);

        // 스피드 적용
        movement *= Manager.Stat.MoveSpeed;

        // 땅이라면 땅위치에서 살짝 띄어줌
        if(Manager.IsGrounded)
        {
            // 점프중일경우 점프중이 아니라고 알림
            if(Manager.IsJumping)
                Manager.IsJumping = false;

            m_ray.origin = transform.position + m_rayOriginY;
            if (Physics.Raycast(m_ray, out m_hit, m_rayDistance, m_ignoreLayerMask))
            {
                m_onGroundPos = transform.position;
                m_onGroundPos.y = m_hit.point.y + m_onGroundUpPos.y;

                transform.position = m_onGroundPos;
            }
        }
        // 땅이 아니라면 중력 적용
        else
        {
            float nextVelocity = m_rigidbody.velocity.y + Manager.Stat.Gravity * Time.deltaTime;
            movement.y = nextVelocity;
        }

        // 이동
        m_rigidbody.velocity = movement;
    }

    // 점프
    private void Jump()
    {
        // 스페이스바를 누르고 땅일경우 점프
        if(Input.GetKeyDown(KeyCode.Space) && Manager.IsGrounded)
        {
            m_rigidbody.AddForce(Vector3.up * Manager.Stat.JumpPower);
            Manager.IsJumping = true;
        }
    }

    // 애니메이션 설정
    private void SetAni()
    {
        m_animator.SetBool("IsRunning", Manager.IsRunning);
        m_animator.SetBool("IsJumping", Manager.IsJumping);
    }
}
