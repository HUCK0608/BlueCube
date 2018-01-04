using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    // 시점변환 키
    [SerializeField]
    private KeyCode m_changeViewKey;
    // 상호작용 키
    [SerializeField]
    private KeyCode m_interactionKey;
    public KeyCode InteractionKey { get { return m_interactionKey; } }

    // 조준 키 (3D 에서만 사용)
    [SerializeField]
    private KeyCode m_scopedKey;
    public KeyCode ScopedKey { get { return m_scopedKey; } }
    // 파이어볼 발사 키
    [SerializeField]
    private KeyCode m_shootFireKey;
    public KeyCode ShootFireKey { get { return m_shootFireKey; } }

    // 플레이어들 2D, 3D
    private GameObject m_player2D, m_player3D;
    private Dictionary<E_ViewType, Player> m_players;

    // 스탯
    private PlayerStat m_stat;
    public PlayerStat Stat { get { return m_stat; } }

    // 점프중인지
    private bool m_isGrounded;
    public bool IsGrounded { get { return m_isGrounded; } set { m_isGrounded = value; } }

    // 중력을 사용할 것인지
    private Coroutine m_gravityOffWhileCor;
    private bool m_useGravity;
    public bool UseGravity { get { return m_useGravity; } set { m_useGravity = true; } }

    private void Awake()
    {
        m_stat = GetComponent<PlayerStat>();
    }

    private void Start()
    {
        InitPlayer();
    }

    private void InitPlayer()
    {
        m_players = new Dictionary<E_ViewType, Player>();

        // 리스트 저장
        m_players.Add(E_ViewType.View2D, transform.Find("2D").GetComponent<Player2D>());
        m_players.Add(E_ViewType.View3D, transform.Find("3D").GetComponent<Player3D>());

        m_player2D = m_players[E_ViewType.View2D].gameObject;
        m_player3D = m_players[E_ViewType.View3D].gameObject;

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
            m_player3D.SetActive(false);
            // 2D 플레이어의 부모를 그룹의 루트로 변경
            m_player2D.transform.parent = transform;
            // 2D 플레이어의 각도를 0으로 만듬
            m_player2D.transform.eulerAngles = Vector3.zero;
            // 3D 캐릭터의 부모를 2D 플레이어로 변경
            m_player3D.transform.parent = m_player2D.transform;
            // 2D 플레이어 활성화
            m_player2D.SetActive(true);
        }
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            // 2D 플레이어 끄기
            m_player2D.SetActive(false);
            // 3D 플레이어의 부모를 그룹의 루트로 변경
            m_player3D.transform.parent = transform;
            // 2D 플레이어의 부모를 3D 플레이어로 변경
            m_player2D.transform.parent = m_player3D.transform;
            // 3D 플레이어 활성화
            m_player3D.SetActive(true);
        }
    }

    public void HitAndRespawn(int damage, Vector3 respawnPoint)
    {
        m_stat.Hit(damage);

        // 각 시점에 맞는 캐릭터를 스폰장소로 이동
        if (GameManager.Instance.ViewType == E_ViewType.View2D)
            m_player2D.transform.position = respawnPoint + new Vector3(0, 1, 0);
        else if (GameManager.Instance.ViewType == E_ViewType.View3D)
            m_player3D.transform.position = respawnPoint + new Vector3(0, 1, 0);
    }
}
