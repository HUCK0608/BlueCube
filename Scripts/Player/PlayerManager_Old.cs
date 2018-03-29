using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager_Old : MonoBehaviour
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
    private PlayerStat_Old m_stat;
    public PlayerStat_Old Stat { get { return m_stat; } }

    private PlayerInventory m_inventory;
    public PlayerInventory Inventory { get { return m_inventory; } }

    // 시점변환 스킬
    private PlayerSkill_ChangeView_Old m_skill_CV;
    public PlayerSkill_ChangeView_Old Skill_CV { get { return m_skill_CV; } }

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
        m_stat = GetComponent<PlayerStat_Old>();
        m_inventory = GetComponent<PlayerInventory>();
        m_skill_CV = GetComponent<PlayerSkill_ChangeView_Old>();
		InitPlayer();
    }

    private void Start()
    {
    }

    private void InitPlayer()
    {
        m_player2D_GO = transform.Find("Player2D").gameObject;
        m_player3D_GO = transform.Find("Player3D").gameObject;

        m_player2D_S = m_player2D_GO.GetComponent<Player2D>();
        m_player3D_S = m_player3D_GO.GetComponent<Player3D>();

        // 플레이어 설정
        ChangePlayer();

        // 중력사용
        m_useGravity = true;
    }

    // 시점변환 때 잠시동안 중력을 끄는 코루틴
    //private IEnumerator GravityOffWhile()
    //{
    //    m_useGravity = false;
    //    yield return new WaitForSeconds(2f);
    //    m_useGravity = true;
    //}

    // 플레이어 변경
    public void ChangePlayer()
    {
        if(m_skill_CV.ViewType.Equals(E_ViewType.View2D))
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
        else if(m_skill_CV.ViewType.Equals(E_ViewType.View3D))
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

    public void AddForce(Vector3 force, ForceMode mode)
    {
        // 2D일 경우
        if(m_skill_CV.ViewType.Equals(GameLibrary.Enum_View2D))
        {

        }
        // 3D일 경우
        else
        {
            m_player3D_S.AddForce(force, mode);
        }
    }

    public void HitAndRespawn(int damage, Vector3 respawnPoint)
    {
        m_stat.Hit(damage);

        // 각 시점에 맞는 캐릭터를 스폰장소로 이동
        if (m_skill_CV.ViewType == E_ViewType.View2D)
            m_player2D_GO.transform.position = respawnPoint + new Vector3(0, 1, 0);
        else if (m_skill_CV.ViewType == E_ViewType.View3D)
            m_player3D_GO.transform.position = respawnPoint + new Vector3(0, 1, 0);
    }
}
