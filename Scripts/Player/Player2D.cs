using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_LookDirection2D { Right = 1, Left = -1 }

public sealed class Player2D : Player
{
    // 리지드바디
    Rigidbody2D m_rigidbody2D;

    // 캐릭터가 바라보는 방향
    private E_LookDirection2D m_lookDirection;
    public E_LookDirection2D LookDirection { get { return m_lookDirection; } }

    protected override void Awake()
    {
        base.Awake();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_lookDirection = E_LookDirection2D.Right;
    }

    // 점프중 시점전환 할 때 점프속도를 넘기기위한 함수
    public override float VelocityY
    {
        get
        {
            return m_rigidbody2D.velocity.y;
        }

        set
        {
            m_rigidbody2D.velocity = new Vector2(0, value);
        }
    }

    private void Update()
    {
        Move();
        Jump();
    }

    // 2D 이동
    private void Move()
    {
        // 키보드 입력
        float move = Input.GetAxis("Horizontal") * Manager.Stat.MoveSpeed;

        // 중력을 사용중이지 않을 때
        if (!Manager.UseGravity)
        {
            // 이동키를 누를경우 중력 사용
            if (move != 0)
                Manager.UseGravity = true;
        }

        // 바라보는 방향 변경해주기
        if (m_lookDirection != E_LookDirection2D.Right && move > 0)
        {
            m_lookDirection = E_LookDirection2D.Right;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (m_lookDirection != E_LookDirection2D.Left && move < 0)
        {
            m_lookDirection = E_LookDirection2D.Left;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // 이동벡터 구하기
        Vector3 movement = Vector3.right * move;

        // 땅이 아니고 중력을 사용한다면 중력적용
        if(!Manager.IsGrounded && Manager.UseGravity)
        {
            float nextVelocity = m_rigidbody2D.velocity.y + Manager.Stat.Gravity * Time.deltaTime;
            movement.y = nextVelocity;
        }

        // 이동
        m_rigidbody2D.velocity = movement;
    }

    // 점프
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Manager.IsGrounded)
        {
            // 중력사용을 하지 않을 때 점프를 할 경우 중력사용
            if (!Manager.UseGravity)
                Manager.UseGravity = true;

            m_rigidbody2D.AddForce(Vector2.up * Manager.Stat.JumpPower);
            Manager.IsGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player" && other.tag != "Respawn")
        {
            // 바닥에 닿았을 경우 매니저에 알려줌
            Manager.IsGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player" && other.tag != "Respawn")
        {
            // 점프상태가 아닌데 바닥에 아무것도 안닿는 경우 중력적용을 위해 설정 변경
            if (Manager.IsGrounded)
                Manager.IsGrounded = false;
        }
    }
}
