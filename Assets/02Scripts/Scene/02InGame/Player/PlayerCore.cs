using Photon.Pun;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public static PlayerCore Instance { get; private set; }

    public PlayerInputManager m_inputManager { get; private set; }
    public PlayerAnimationManager m_animationManager { get; private set; }
    public PlayerLocomotion m_locomotion { get; private set; }
    public CameraManager m_cameraManager;

    public PhotonView m_photonView { get; private set; }
    private void Awake()
    {
        if (m_inputManager == null) m_inputManager = GetComponent<PlayerInputManager>();
        if (m_animationManager == null) m_animationManager = GetComponentInChildren<PlayerAnimationManager>();
        if (m_locomotion == null) m_locomotion = GetComponent<PlayerLocomotion>();
        if (m_cameraManager == null) m_cameraManager = GetComponentInChildren<CameraManager>();

        m_photonView = GetComponent<PhotonView>();

        m_locomotion.Initialize(this);
        m_animationManager.Initialize(this);
        m_cameraManager.Initialize(this);
    }
    
    private void Start()
    {
        // �ʱ�ȭ �۾�
        // ��: �ִϸ��̼� �Ŵ��� �ʱ�ȭ, �Է� �Ŵ��� ���� ��
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // �̱��� ������ ���� �ߺ� ���� ����
        
    }

    private void FixedUpdate()
    {
        // ���� ��� ���� ó��
        // ��: �̵�, ����, ��� ��
        if (!m_photonView.IsMine) return;
        m_locomotion.HandleMove(m_inputManager.m_MoveInput);
    }

    private void Update()
    {
        

    }

    private void LateUpdate()
    {
        // �ִϸ��̼� ������Ʈ
        // ��: �ִϸ��̼� ���� ������Ʈ, �̵� �ӵ� ���� ��
        if (!m_photonView.IsMine) return;
        m_locomotion.HandleRotation(m_cameraManager.m_Dir);
        m_cameraManager.OnCameraRotation(m_inputManager.m_LookInput);
        m_animationManager.HandleAnimation(m_inputManager, m_locomotion);
    }

}
