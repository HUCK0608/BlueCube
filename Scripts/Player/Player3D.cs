using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player3D : Player
{
    // 애니메이터
    private Animator m_animator;

    // 리지드바디
    private Rigidbody m_rigidbody;

    protected override void Awake()
    {
        base.Awake();
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
        SetAni();
    }

    // 3D 이동
    private void Move()
    {
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
                movement = Vector3.forward;
            // 하, 우
            else if (moveX > 0 && moveZ < 0)
                movement = Vector3.right;
            // 하, 좌
            else if (moveX < 0 && moveZ < 0)
                movement = Vector3.back;
            // 상, 좌
            else if (moveX < 0 && moveZ > 0)
                movement = Vector3.left;
        }
        // 한개의 키가 눌렸을 경우
        else if (moveX != 0 || moveZ != 0)
        {
            // 상
            if (moveZ > 0)
                movement = Vector3.forward + Vector3.left;
            // 하
            else if (moveZ < 0)
                movement = Vector3.back + Vector3.right;
            // 좌
            else if (moveX < 0)
                movement = Vector3.back + Vector3.left;
            // 우
            else if (moveX > 0)
                movement = Vector3.forward + Vector3.right;
        }

        // 이동을 하는 경우에만 캐릭터 회전
        if (movement != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Manager.Stat.RotationSpeed * Time.deltaTime);

        // 스피드 적용
        movement *= Manager.Stat.MoveSpeed;

        // 중력을 사용한다면 중력적용
        if (Manager.UseGravity)
        {
            // 점프 상태이거나 땅이 아니면 중력 적용
            if (Manager.IsJumping || !Manager.IsGrounded)
            {
                float nextVelocity = m_rigidbody.velocity.y + Manager.Stat.Gravity * Time.deltaTime;
                movement.y = nextVelocity;
            }
        }

        // 이동
        m_rigidbody.velocity = movement;
    }

    // 점프
    private void Jump()
    {
        // 스페이스바를 누르고 땅이고 점프상태가 아닐 때 점프
        if(Input.GetKeyDown(KeyCode.Space) && Manager.IsGrounded && !Manager.IsJumping)
        {
            // 중력사용을 하지 않을 때 점프를 할 경우 중력사용
            if (!Manager.UseGravity)
                Manager.UseGravity = true;

            m_rigidbody.AddForce(Vector3.up * Manager.Stat.JumpPower);
            Manager.IsGrounded = false;
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
