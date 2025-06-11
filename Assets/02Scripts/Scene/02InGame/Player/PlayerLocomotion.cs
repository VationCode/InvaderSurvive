// �Ϲ����� �ൿ ��� Ŭ����

using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLocomotion : MonoBehaviour
{


    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider m_collider;
    [SerializeField] float m_wolkSpeed = 3f;
    [SerializeField] float m_runSpeed = 6f;
    [SerializeField] float m_crouchSpeed = 2.5f;
    [SerializeField] float m_coruchRunSpeed = 4f;

    [SerializeField] float rotationSpeed = 10f;

    [SerializeField] LayerMask m_groundMask;
    [SerializeField] float m_jumpForce = 10f;
    [SerializeField] float m_jumpForceDelay = 0.3f; // ���� ������ �ð�
    [SerializeField] int m_maxJumpCount = 2;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] float gravity = -9.81f; // �߷� ��
    [SerializeField] float m_groundCheckDistance = 0.1f;

    [SerializeField] float dashForce = 10f;
    [SerializeField] float slideForce = 3f;

    public bool m_IsRun; //{ get; private set; }
    public bool m_IsCrouch; //{ get; private set; }
    public bool m_IsCrouchRun;
    public bool m_IsJump; //{ get; private set; }
    public bool m_IsDodge; //{ get; private set; }
    public bool m_IsSlide; //{ get; private set; }


    private PlayerCore m_playerCore;
    private bool m_isGrounded; // ���� ����ִ��� ����
    private float m_velocityY;

    // 2�� ���� ���� ����
    private int m_currentJumpCount = 0;
    private bool m_jumpHeld = false;
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (m_collider == null)
        {
            m_collider = GetComponent<CapsuleCollider>();
        }
    }

    public void Initialize(PlayerCore core)
    {
        this.m_playerCore = core;
    }

    private void Start()
    {
        if (!m_playerCore.m_photonView.IsMine) return;
        rb.freezeRotation = true; // Rigidbody ȸ�� ����
        m_playerCore.m_inputManager.IsRun += () => m_IsRun = !m_IsRun; // Run ���
        m_playerCore.m_inputManager.IsCrouch += () => m_IsCrouch = !m_IsCrouch; // Crouch ���
        m_playerCore.m_inputManager.IsJump += HandleJump; // Jump ���
    }

    private void FixedUpdate()
    {
        if (!m_playerCore.m_photonView.IsMine) return;
        ApplyCustomGravity();
    }

    private void Update()
    {
        if (!m_playerCore.m_photonView.IsMine) return;
        UpdateGroundCheck();
    }

    #region ======================================================= Movement
    // HandleMove
    public void HandleMove(Vector2 inputMove)
    {
        Vector3 dir = transform.right * inputMove.x + transform.forward * inputMove.y;
        dir = dir.normalized;


        float speed = m_IsRun ? m_runSpeed : m_wolkSpeed;
        if (m_IsCrouch)
        {
            speed = m_IsRun ? m_coruchRunSpeed : m_crouchSpeed;
            m_IsCrouchRun = m_IsRun; // ��ũ�� ���¿��� �޸��� ����
        }
        else m_IsCrouchRun = false;

        Vector3 velocity = rb.linearVelocity;
        if (dir.magnitude > 0.1f)
        {
            velocity.x = dir.x * speed;
            velocity.z = dir.z * speed; 
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
        }
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    // Rotate
    public void HandleRotation(Vector3 cameraDir)
    {
        Quaternion _targetRotation = Quaternion.Euler(cameraDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Jump
    public void HandleJump()
    {
        // 2�� ���� ó��
        if (m_isGrounded)
        {
            m_IsCrouch = false;
            m_IsCrouchRun = false;
            Jump();
        }
        else if(m_currentJumpCount < m_maxJumpCount-1) Jump();
    }

    private void Jump()
    {
        m_playerCore.m_animationManager.HandleJumpAni();
        m_currentJumpCount++;
        Invoke("InvokeJump", m_jumpForceDelay);
    }

    // �ִϸ��̼� Ÿ�̹� ���߱� ����
    private void InvokeJump()
    {
        Invoke("IsInAir", 0.2f);
        Vector3 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;
        rb.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
    }

    // Ground����
    private void IsInAir()
    {
        m_IsJump = true;
        m_playerCore.m_animationManager.HandleInAir(true);
    }

    private void ApplyCustomGravity()
    {
        // ���� ��ư�� ª�� ������ ���� ����, ������ �� �� ������ ����
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !m_jumpHeld)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        else
        {
            rb.linearVelocity += Physics.gravity * Time.fixedDeltaTime;
        }
    }

    private void UpdateGroundCheck()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + m_collider.center;
        float radius = m_collider.radius*0.9f; // �ݶ��̴� �������� 90% ���
        float distance = m_collider.center.y - radius + 0.05f;

        // �ش� �������� �Ʒ��� Raycast�� ���� ���� ����ִ��� Ȯ�� ���� ��� ���� ��� �Ǵ��� ���� �����
        // �׽�Ʈ ��� ���� �����鼭 ���� ����� Ȯ���ϴµ� �̹� ���� ����� ���� ����ִ� ���·� �����ϸ� �Ǵ� �����
        m_isGrounded = Physics.SphereCast(origin, radius, Vector3.down, out hit, distance, m_groundMask);
        
        // �Ǵ� ���� ������� �Ʒ���ĵ� ����
        // m_isGrounded = Physics.CheckSphere(origin,radius,m_groundMask);

        if (m_isGrounded)
        {
            m_IsJump = false;
            m_currentJumpCount = 0;
            m_jumpHeld = false;
            m_playerCore.m_animationManager.HandleInAir(false);
        }
    }
    // Dash

    // Crouch

    // Slide

    #endregion ======================================================= /Movement
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + m_collider.center;
        float radius = m_collider.radius * 0.9f; // �ݶ��̴� �������� 90% ���
        float distance = m_collider.center.y - radius + 0.05f; // 0.2~0.3f ����
        origin.y -= distance;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin, radius);
    }
}
