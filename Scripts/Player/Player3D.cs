using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : Player
{
    // 리지드바디
    private Rigidbody m_rigidbody;

    protected override void Awake()
    {
        base.Awake();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // 점프중 시점전환 할 때 점프속도를 넘기기위한 함수
    public override float VelocityY
    {
        get
        {
            return m_rigidbody.velocity.y;
        }

        set
        {
            m_rigidbody.velocity = new Vector3(0, value, 0);
        }
    }

    private void Update()
    {
        Scoped();
        Move();
        Jump();
    }

    // 3D 이동
    private void Move()
    {
        // 카메라그룹의 y축 각도를 가져옴
        float cameraYAngle = GameManager.Instance.CameraManager.GetYAngle3D();

        // 캐릭터 회전
        transform.eulerAngles = new Vector3(0, cameraYAngle, 0);

        // 키입력
        float moveX = Input.GetAxis("Horizontal") * Manager.Stat.MoveSpeed;
        float moveZ = Input.GetAxis("Vertical") * Manager.Stat.MoveSpeed;

        // 중력을 사용중이지 않을 때
        if(!Manager.UseGravity)
        {
            // 이동키를 누를경우 중력 사용
            if (moveX != 0 || moveZ != 0)
                Manager.UseGravity = true;
        }

        // 벡터 형태로 변경
        Vector3 movement = transform.forward * moveZ + transform.right * moveX;

        // 땅이 아니고 중력을 사용한다면 중력적용
        if(!Manager.IsGrounded && Manager.UseGravity)
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
            // 중력사용을 하지 않을 때 점프를 할 경우 중력사용
            if (!Manager.UseGravity)
                Manager.UseGravity = true;

            m_rigidbody.AddForce(Vector3.up * Manager.Stat.JumpPower);
            Manager.IsGrounded = false;
        }
    }

    // 카메라 줌 인 아웃
    private void Scoped()
    {
        // 줌 키를 눌렀을 경우 줌(줌키는 플레이어 매니저에 있음)
        if (Input.GetKeyDown(Manager.ScopedKey))
            GameManager.Instance.CameraManager.ChangeScoped();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Respawn")
        {
            // 바닥에 닿았을 경우 매니저에 알려줌
            Manager.IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Respawn")
        {
            // 점프상태가 아닌데 바닥에 아무것도 안닿는 경우 중력적용을 위해 설정 변경
            if (Manager.IsGrounded)
                Manager.IsGrounded = false;
        }
    }
}
