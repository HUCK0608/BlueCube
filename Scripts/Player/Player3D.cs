using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player3D : Player
{
    // 애니메이터
    private Animator m_animator;
    // 리지드바디
    private Rigidbody m_rigidbody;
    // 착지위치 오브젝트
    private GameObject m_landingPoint;

    // 애니메이션 전용 방향
    private int m_aniDirection;

    // 땅 체크를 위한 변수들
    private List<Transform> m_checkGroundPoints;    // 각 체크 위치 오브젝트를 담은 변수
    private Vector3 m_rayOriginY;                   // 플레이어위치에서 y축으로 살짝 띄워 발사하기 위한 변수
    private Vector3 m_onGroundPos;                  // 실직적인 땅에서 떨어진 플레이어 위치를 담을 변수
    private float m_onGroundUpPosY;                 // 플레이어위치에 더해줄 위치가 담긴 변수
    private float m_checkGroundRayDis;              // 땅 체크 레이 발사 길이

    // 레이사용 공통 변수
    private Ray m_ray;
    private RaycastHit m_hit;                       // 레이 충돌 정보를 담을 변수

    [SerializeField]
    private float m_ladderCheckDistance;

    private bool m_onLadder;

    protected override void Awake()
    {
        base.Awake();

        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_landingPoint = transform.parent.Find("LandingPoint3D").gameObject;

        m_ray = new Ray();

        InitCheckGround();
    }

    // 땅체크를 위한 초기화
    private void InitCheckGround()
    {
        // 각 꼭지점 transform 가져오기
        m_checkGroundPoints = new List<Transform>();

        Transform checkGroundPoints = transform.Find("CheckGroundPoints");

        int checkGroundPointCount = checkGroundPoints.transform.childCount;

        for (int i = 0; i < checkGroundPointCount; i++)
            m_checkGroundPoints.Add(checkGroundPoints.GetChild(i));

        // 레이의 시작 y 위치
        m_rayOriginY = new Vector3(0, 0.1f, 0);

        // 플레이어를 올릴 땅 위 위치
        m_onGroundUpPosY = 0.05f;

        // 레이 거리
        m_checkGroundRayDis = 0.16f;
    }

    private void Update()
    {
        CheckGround();
        RotationToMousePoint();
        Jump();
        Move();
        DrawLandingPoint();
    }

    // 땅인지 체크하는 함수
    private void CheckGround()
    {
        // 점프중일경우 최고높이까지 갈 때 까지 땅 체크를 하지 않음
        if (m_playerManager.IsJumping && m_rigidbody.velocity.y >= 0f)
        {
            if (m_playerManager.IsGrounded)
                m_playerManager.IsGrounded = false;

            return;
        }

        // 무시해야할 총알, 플레이어 레이어마스크
        int ignoreLM = GameLibrary.LayerMask_Ignore_BP;

        // 각 콜라이더 꼭지점 레이 체크
        // 한 점이라도 충돌된 것이 있으면 땅이라고 적용
        // 4점 모두 충돌된 것이 없을 경우에만 땅이 아니라고 적용

        // 아래방향 발사 공통
        m_ray.direction = Vector3.down;

        // 상
        m_ray.origin = m_checkGroundPoints[0].position + m_rayOriginY;
        
        Debug.DrawRay(m_ray.origin, m_ray.direction, Color.red);

        if (Physics.Raycast(m_ray, m_checkGroundRayDis, ignoreLM))
            m_playerManager.IsGrounded = true;
        else
            m_playerManager.IsGrounded = false;

        // 하
        m_ray.origin = m_checkGroundPoints[1].position + m_rayOriginY;
        Debug.DrawRay(m_ray.origin, m_ray.direction, Color.red);

        if (Physics.Raycast(m_ray, m_checkGroundRayDis, ignoreLM))
            m_playerManager.IsGrounded = true;
        else if (!m_playerManager.IsGrounded)
            m_playerManager.IsGrounded = false;

        // 좌
        m_ray.origin = m_checkGroundPoints[2].position + m_rayOriginY;
        Debug.DrawRay(m_ray.origin, m_ray.direction, Color.red);

        if (Physics.Raycast(m_ray, m_checkGroundRayDis, ignoreLM))
            m_playerManager.IsGrounded = true;
        else if(!m_playerManager.IsGrounded)
            m_playerManager.IsGrounded = false;

        // 우
        m_ray.origin = m_checkGroundPoints[3].position + m_rayOriginY;
        Debug.DrawRay(m_ray.origin, m_ray.direction, Color.red);

        if (Physics.Raycast(m_ray, m_checkGroundRayDis, ignoreLM))
            m_playerManager.IsGrounded = true;
        else if(!m_playerManager.IsGrounded)
            m_playerManager.IsGrounded = false;

        // 땅이라면 땅위치에서 살짝 띄어줌
        if (m_playerManager.IsGrounded)
        {
            // 점프중일경우 점프중이 아니라고 알림
            if (m_playerManager.IsJumping)
                m_playerManager.IsJumping = false;

            m_ray.origin = transform.position + m_rayOriginY;

            if (Physics.Raycast(m_ray, out m_hit, m_checkGroundRayDis, GameLibrary.LayerMask_Ignore_BP))
            {
                // 플레이어를 띄울 위치를 계산
                m_onGroundPos = transform.position;
                m_onGroundPos.y = m_hit.point.y + m_onGroundUpPosY;

                // 띄울 위치 적용
                transform.position = m_onGroundPos;
            }
        }
    }

    // 3D 이동
    private void Move()
    {
        // 시점변환중이거나 관찰시점이면 리턴
        if (GameLibrary.Bool_IsCO)
        {
            m_rigidbody.velocity = Vector3.zero;
            return;
        }

        // 키입력
        float moveX = Input.GetAxis("Horizontal") * m_playerManager.Stat.ForwardMoveSpeed;
        float moveZ = Input.GetAxis("Vertical") * m_playerManager.Stat.ForwardMoveSpeed;

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

        movement = movement.normalized;

        bool checkLadder = CheckLadder(movement);

        // 사다리가 있을경우
        if (!checkLadder)
        {
            // 애니메이션 변수 설정
            SetAni(movement);

            // 가는 방향이 정면일 경우 정면 스피드 적용
            if (m_aniDirection.Equals(0))
                movement *= m_playerManager.Stat.ForwardMoveSpeed;
            // 정면이 아닐경우 조금 느린 스피드 적용
            else
                movement *= m_playerManager.Stat.SideBackMoveSpeed;

            // 땅이아니거나 점프중이거나 사다리위가 아닐경우 중력적용
            if (!m_playerManager.IsGrounded || m_playerManager.IsJumping)
            {
                float nextVelocity = m_rigidbody.velocity.y + m_playerManager.Stat.Gravity * Time.deltaTime;
                movement.y = nextVelocity;
            }
        }
        // 사다리가 없을경우
        else
        {
            movement = Vector3.up * m_playerManager.Stat.ForwardMoveSpeed;
        }

        // 이동
        m_rigidbody.velocity = movement;
    }

    // 사다리가 있는지 체크
    private bool CheckLadder(Vector3 direction)
    {
        m_ray.origin = transform.position + Vector3.up * 0.5f;
        m_ray.direction = direction;

        // 사다리만 통과하는 레이어마스크
        int layerMask = GameLibrary.LayerMask_Ladder;

        Debug.DrawRay(m_ray.origin, m_ray.direction * m_ladderCheckDistance, Color.yellow);
        if (Physics.Raycast(m_ray, m_ladderCheckDistance, layerMask))
        {
            return true;
        }

        return false;
    }

    // 점프
    private void Jump()
    {
        // 스페이스바를 누르고 땅일경우 점프
        if(Input.GetKeyDown(KeyCode.Space) && m_playerManager.IsGrounded)
        {
            m_rigidbody.AddForce(Vector3.up * m_playerManager.Stat.JumpPower);
            m_playerManager.IsJumping = true;
        }
    }

    // 마우스위치로 회전
    private void RotationToMousePoint()
    {
        // 시점변환중이거나 관찰시점이면 리턴
        if (GameLibrary.Bool_IsCO)
            return;

        // 마우스 방향 구하기
        Vector3 mouseDirection = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

        transform.rotation = Quaternion.LookRotation(mouseDirection);
    }

    // 착지지점을 표시
    private void DrawLandingPoint()
    {
        // 땅일경우
        if(m_playerManager.IsGrounded)
        {
            // 착지지점이 활성화 되어있을 경우
            if (m_landingPoint.activeSelf)
                // 비활성화
                m_landingPoint.SetActive(false);
        }
        // 땅이 아닐경우
        else
        {
            // 레이 시작장소 및 방향 설정
            m_ray.origin = transform.position + m_rayOriginY;
            m_ray.direction = Vector3.down;

            // 레이발사
            // 레이에 충돌체가 있을경우
            if(Physics.Raycast(m_ray, out m_hit, Mathf.Infinity, GameLibrary.LayerMask_Ignore_BP))
            {
                // 착지지점이 활성화 되어있지 않을 경우
                if (!m_landingPoint.activeSelf)
                    // 활성화
                    m_landingPoint.SetActive(true);

                // 위치 변경
                m_landingPoint.transform.position = m_hit.point;
            }
            // 레이에 충돌체가 없을경우
            else
            {
                // 착지지점이 활성화 되어있을 경우
                if (m_landingPoint.activeSelf)
                    // 비활성화
                    m_landingPoint.SetActive(false);
            }
        }
    }


    // 애니메이션 설정
    private void SetAni(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            float angle = Vector3.Angle(transform.forward, moveDirection);
            Vector3 cross = Vector3.Cross(transform.forward, moveDirection);

            // 부호결정
            if (cross.y < 0) angle = -angle;

            // Forward
            if (angle >= -50f && angle <= 50f)
                m_aniDirection = 0;
            // Left
            else if (angle > -130f && angle < -50f)
                m_aniDirection = 2;
            // Right
            else if (angle > 50f && angle < 130f)
                m_aniDirection = 3;
            // Back
            else
                m_aniDirection = 1;

        }

        // 애니메이션 설정변수
        if (m_rigidbody.velocity.x.Equals(0) && m_rigidbody.velocity.z.Equals(0))
            m_playerManager.IsRunning = false;
        else
            m_playerManager.IsRunning = true;

        
        m_animator.SetBool("IsRunning", m_playerManager.IsRunning);
        m_animator.SetBool("IsJumping", m_playerManager.IsJumping);
        m_animator.SetInteger("Direction", m_aniDirection);
    }

    // 캐릭터에 힘주기
    public void AddForce(Vector3 force, ForceMode mode)
    {
        m_rigidbody.AddForce(force, mode);
    }
}
