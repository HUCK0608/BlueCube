using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    // 시점변환 키
    [SerializeField]
    private KeyCode m_changeViewKey;
    public KeyCode ChangeViewKey { get { return m_changeViewKey; } }

    // 상호작용 키
    [SerializeField]
    private KeyCode m_interactionKey;
    public KeyCode InteractionKey { get { return m_interactionKey; } }

    // 파이어볼 발사 키
    [SerializeField]
    private KeyCode m_shootFireKey;
    public KeyCode ShootFireKey { get { return m_shootFireKey; } }

    // 플레이어들 2D, 3D 오브젝트
    private GameObject m_player2D_GO;
    private GameObject m_player3D_GO;
    public GameObject Player2D_GO { get { return m_player2D_GO; } }
    public GameObject Player3D_GO { get { return m_player3D_GO; } }

    // 플레이어 2D, 3D 스크립트
    private Player2D m_player2D_S;
    private Player3D m_player3D_S;
    public Player2D Player2D_S { get { return m_player2D_S; } }
    public Player3D Player3D_S { get { return m_player3D_S; } }

    // 스탯
    private PlayerStat m_stat;
    public PlayerStat Stat { get { return m_stat; } }

    // 시점변환 스킬
    private PlayerSkill_ChangeView m_skill_CV;
    public PlayerSkill_ChangeView Skill_CV { get { return m_skill_CV; } }

    // 점프중인지
    private bool m_isGrounded;
    public bool IsGrounded { get { return m_isGrounded; } set { m_isGrounded = value; } }

    // 중력을 사용할 것인지
    private Coroutine m_gravityOffWhileCor;
    private bool m_useGravity;
    public bool UseGravity { get { return m_useGravity; } set { m_useGravity = true; } }

    // 애니메이터 사용
    private bool m_isRunning;
    public bool IsRunning { get { return m_isRunning; } set { m_isRunning = value; } }

    // 점프중인지
    private bool m_isJumping;
    public bool IsJumping { get { return m_isJumping; } set { m_isJumping = value; } }

    private void Awake()
    {
        m_stat = GetComponent<PlayerStat>();
        m_skill_CV = GetComponent<PlayerSkill_ChangeView>();
    }

    private void Start()
    {
        InitPlayer();
    }

    private void InitPlayer()
    {
        m_player2D_GO = transform.Find("2D").gameObject;
        m_player3D_GO = transform.Find("3D").gameObject;

        m_player2D_S = m_player2D_GO.GetComponent<Player2D>();
        m_player3D_S = m_player3D_GO.GetComponent<Player3D>();

        // 플레이어 설정
        ChangePlayer();

        // 중력사용
        m_useGravity = true;
    }

    private void Update()
    {
        // 시점변환 키를 눌렀을 경우
        if(Input.GetKeyDown(m_changeViewKey))
        {
            // 코루틴 종료
            if (m_gravityOffWhileCor != null)
                StopCoroutine(m_gravityOffWhileCor);

            // 시점변환
            GameManager.Instance.ChangeViewType();

            // 잠시 중력 끄기
            m_gravityOffWhileCor = StartCoroutine(GravityOffWhile());
        }
    }

    // 시점변환 때 잠시동안 중력을 끄는 코루틴
    private IEnumerator GravityOffWhile()
    {
        m_useGravity = false;
        yield return new WaitForSeconds(2f);
        m_useGravity = true;
    }

    // 플레이어 변경
    public void ChangePlayer()
    {
        if(GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            // 3D 플레이어 끄기
            m_player3D_GO.SetActive(false);
            // 2D 플레이어의 부모를 그룹의 루트로 변경
            m_player2D_GO.transform.parent = transform;
            // 2D 플레이어의 각도를 0으로 만듬
            m_player2D_GO.transform.eulerAngles = Vector3.zero;
            // 3D 캐릭터의 부모를 2D 플레이어로 변경
            m_player3D_GO.transform.parent = m_player2D_GO.transform;
            // 2D 플레이어 활성화
            m_player2D_GO.SetActive(true);
        }
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            // 2D 플레이어 끄기
            m_player2D_GO.SetActive(false);
            // 3D 플레이어의 부모를 그룹의 루트로 변경
            m_player3D_GO.transform.parent = transform;
            // 2D 플레이어의 부모를 3D 플레이어로 변경
            m_player2D_GO.transform.parent = m_player3D_GO.transform;
            // 스케일 재설정
            m_player3D_GO.transform.localScale = Vector3.one;
            // 3D 플레이어 활성화
            m_player3D_GO.SetActive(true);
        }
    }

    public void Hit(int damage)
    {
        m_stat.Hit(damage);
    }

    public void HitAndRespawn(int damage, Vector3 respawnPoint)
    {
        m_stat.Hit(damage);

        // 각 시점에 맞는 캐릭터를 스폰장소로 이동
        if (GameManager.Instance.ViewType == E_ViewType.View2D)
            m_player2D_GO.transform.position = respawnPoint + new Vector3(0, 1, 0);
        else if (GameManager.Instance.ViewType == E_ViewType.View3D)
            m_player3D_GO.transform.position = respawnPoint + new Vector3(0, 1, 0);
    }
}
