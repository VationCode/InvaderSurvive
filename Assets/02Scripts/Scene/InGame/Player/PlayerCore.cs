// ========================================250415
// 

// ========================================250410
// Locomotion, Combat�� ���� ���� ���°� �ʿ��� ������ ���µ鿡 ���ؼ� MainState�� ����
// IsAiming, IsSprinting, IsCrouching�� ���� ������ ������ State���� SubFlags�� ����

// ========================================
using UnityEngine;
using DUS.PlayerCore.Locomotion;
using DUS.PlayerCore.Combat;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MainStateAndSubFlagsManager))]
[RequireComponent(typeof(PlayerInputManager))] // InputManager������Ʈ�� ���Ӽ����� PlayerCore�ִ� ���� �ڵ����� �߰�

public class PlayerCore : MonoBehaviour
{
    #region ======================================== Module
    public PlayerInputManager m_InputManager { get; private set; }
    public PlayerLocomotion m_Locomotion { get; private set; }
    public PlayerCombat m_Combat { get; private set; }
    public MainStateAndSubFlagsManager m_StateFlagManager { get; private set; }
    public PlayerAnimationManager m_AnimationManager;
    public CameraRigManager m_CameraManager { get; private set; }
    #endregion ======================================== /Module

    #region ======================================== Player Value - Locomotion
    [Header("[ Move ]")]
    [Range(1, 10), SerializeField] float m_walkSpeed = 5f;
    public float m_WalkSpeed => m_walkSpeed;
    [Range(1, 20),SerializeField] float m_runSpeed = 15f;
    public float m_RunSpeed => m_runSpeed;

    [Range(1, 10), SerializeField] float m_crouchSpeed = 3f;
    public float m_CrouchSpeed => m_crouchSpeed;

    [Range(1, 10), SerializeField] float m_crouchRunSpeed = 9f;
    public float m_CrouchRunSpeed => m_crouchRunSpeed;

    [Range(20, 50), SerializeField] float m_sprintSpeed = 20f;
    public float m_SprintSpeed => m_sprintSpeed;

    [Range(1, 10), SerializeField] float m_dodgeSpeed = 5f;
    public float m_DodgeSpeed => m_dodgeSpeed;

    [Range(1, 10), SerializeField] float m_slideSpeed = 5f;
    public float m_SlideSpeed => m_slideSpeed;

    [Range(1, 10), SerializeField] float m_climbSpeed = 5f;
    public float m_ClimbSpeed => m_climbSpeed;

    [Range(1, 10), SerializeField] float m_wallRunSpeed = 5f;
    public float m_WallRunSpeed => m_wallRunSpeed;

    [Header("[ Jump ]")]
    [Range(5, 20), SerializeField] float m_jumpForce = 8f;
    public float m_JumpForce => m_jumpForce;

    [Range(0.1f, 1.0f), SerializeField] float m_jumpForwardSpeedPercent = 0.6f;
    public float m_JumpForwardSpeedPercent => m_jumpForwardSpeedPercent;
    //[SerializeField] public float m_JumpDuration = 1f;
    //[SerializeField] public AnimationCurve m_JumpCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f); // m_JumpDuration ���� 1 �� 0 �� � ��ȯ

    [Header("[ Gravity ]")]
    [Range(1, 10),SerializeField] float m_addGravity = 1;
    public float m_AddGravity => m_addGravity;

    [SerializeField] LayerMask m_groundMask;
    public LayerMask m_GroundMask => m_groundMask;

    [Range(1, 50), SerializeField] float m_maxVelocityY = 30;
    public float m_MaxVelocityY => m_maxVelocityY;

    [Range(-50, -1),SerializeField] float m_minVelocityY = -30;
    public float m_MinVelocityY => m_minVelocityY;

    [Header("[ PlayerCore Rot ]")]
    [Range(1, 20),SerializeField] float m_rotationSpeed = 10;
    public float m_RotationSpeed => m_rotationSpeed;
    //[Range(1, 60)] public float m_rotationDamping; //ȸ�� ����
    #endregion ======================================== /Player Value Locomotion

    #region ======================================== Player Value - Combat
    [Range(1, 50)] float m_rotationAimSpeed; //���� ���¿����� ȸ�� �ӵ�
    public float m_RotationAimSpeed => m_rotationAimSpeed;

    #endregion ======================================== /Player Value Combat

    public Transform m_TargetFollowCam;
    public Rigidbody m_Rigidbody { get; private set; }
    public CapsuleCollider[] m_CapsuleCollider { get; private set; }
    public float m_CurrentRotSpeed { get; private set; }

    public PhotonView m_photonView;

    //�޾ƿ��� ������ �߿�
    private void Awake()
    {
        //instance = this;
        //m_Locomotion ������ ���� ���� Get���� ����� ã�Ƴ��� �־����

        m_InputManager = GetComponent<PlayerInputManager>(); // �Ǵ� ���� ����
        m_AnimationManager = GetComponentInChildren<PlayerAnimationManager>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CapsuleCollider = GetComponents<CapsuleCollider>();
        m_CameraManager = FindObjectOfType<CameraRigManager>();
        m_StateFlagManager = GetComponent<MainStateAndSubFlagsManager>();

        m_groundMask = LayerMask.GetMask("Ground");
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous; //������ٵ� ���� ��� ����
        Application.targetFrameRate = 300; //Fixed ������ ����

        m_photonView = GetComponent<PhotonView>();

        //������ this�� ���� �ʴ� ������ �����ֱ⿡ ���� ���� �ڵ� ���⵵ �߻��ֱ⿡ Initialize�� ������ ó��



    }
    private void Start()
    {
        m_Locomotion = new PlayerLocomotion(this);
        //m_Combat = new PlayerCombat(this);

        OnChangeColider(true);
        m_Locomotion.InitializeLocomotionAtStart();
        m_CurrentRotSpeed = m_RotationSpeed;

        // �÷��̾�� �ε����� �� ��鸮�� ��Ʈ��ũ �� ���� �߻�
        // �ƿ� �浹�� �ȵǵ��� ����
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerCore"), LayerMask.NameToLayer("PlayerCore"), true);

        //m_Rigidbody.transform.position = Vector3.zero;
    }

    public void FixedUpdate()
    {
        if (!m_photonView.IsMine) return;
        m_Locomotion?.FixedUpdate();
        //m_Combat?.FixedUpdate();
    }
    private void Update()
    {
        if (!m_photonView.IsMine) return;
        m_Locomotion?.Update();
        m_Combat?.Update();
    }

    private void LateUpdate()
    {
        if (!m_photonView.IsMine) return;
        m_Locomotion?.LateUpdate();
    }

    //TODO : Locomotion�� Combat�� ���� ������ �ִ� ���� �߻� �� �����ϴ� ���� �ۼ�
    bool m_IsLocomotion, m_IsCombat;


    #region ======================================== Set Player Value - Locomotion

    public void OnChangeColider(bool isOrigin)
    {
        m_CapsuleCollider[0].enabled = isOrigin;
        m_CapsuleCollider[1].enabled = !isOrigin; //�����̵� �� ���� ����
    }
    #endregion ======================================== /Set Player Value - Locomotion
}
