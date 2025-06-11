using Photon.Pun;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] Animator m_animator;

    private PlayerCore m_playerCore;
    private void Awake()
    {
        if (m_animator == null)
        {
            m_animator = GetComponent<Animator>();
        }
    }

    public void Initialize(PlayerCore core)
    {
        this.m_playerCore = core;
    }
    public void HandleMoveAni(float moveX, float moveY)
    {
        if (moveX == 0f && moveY == 0f)
        {
            m_animator.SetFloat("MoveX", 0);
            m_animator.SetFloat("MoveY", 0);
            m_animator.SetBool("IsMove", false);
            return;
        }
        m_animator.SetBool("IsMove", moveX != 0 || moveY != 0);

        // ������ ����Ͽ� �ִϸ��̼� ��ȯ �ε巴��
        moveX = Mathf.Lerp(m_animator.GetFloat("MoveX"), moveX, Time.deltaTime * 10f);
        moveY = Mathf.Lerp(m_animator.GetFloat("MoveY"), moveY, Time.deltaTime * 10f);

        m_animator.SetFloat("MoveX", moveX);
        m_animator.SetFloat("MoveY", moveY);
    }

    public void HandleRunAni(bool isRunning)
    {
        // �޸��� �ִϸ��̼� ó��
        m_animator.SetBool("IsRun", isRunning);
    }

    public void HandleCrouchAni(bool isCrouching)
    {
        // ��ũ���� �ִϸ��̼� ó��
        m_animator.SetBool("IsCrouch", isCrouching);
    }

    public void HandleCrouchRunAni(bool isCrouchRun)
    {
        m_animator.SetBool("IsCrouchRun", isCrouchRun);
    }

    public void HandleJumpAni()
    {
        m_animator.SetTrigger("IsJump");
    }

    public void HandleInAir(bool isInAir)
    {
        m_animator.SetBool("IsInAir", isInAir);
    }
    public void HandleAttackAni(bool isAttacking)
    {
        // ���� �ִϸ��̼� ó��
        m_animator.SetBool("IsAttack", isAttacking);
    }
    public void HandleDodgeAni(bool isDodging)
    {
        // ȸ�� �ִϸ��̼� ó��
        m_animator.SetBool("IsDodge", isDodging);
    }
    public void HandleSlideAni(bool isSliding)
    {
        // �����̵� �ִϸ��̼� ó��
        m_animator.SetBool("IsSlide", isSliding);
    }

    public void HandleAnimation(PlayerInputManager inputManager, PlayerLocomotion playerLocomotion)
    {
        // �ִϸ��̼� ������Ʈ
        HandleMoveAni(inputManager.m_MoveInput.x, inputManager.m_MoveInput.y);
        HandleRunAni(playerLocomotion.m_IsRun);
        HandleCrouchAni(playerLocomotion.m_IsCrouch);
        HandleCrouchRunAni(playerLocomotion.m_IsCrouchRun);
        HandleAttackAni(inputManager.m_IsAttack);
        HandleDodgeAni(playerLocomotion.m_IsDodge);
        HandleSlideAni(playerLocomotion.m_IsSlide);
    }
}
