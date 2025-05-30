using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum AniParmType
{
    None = 0,
    SetBool,
    SetTrigger,
    SetInt,
    SetFloat
}

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    //Awake�� Locomotion���� ������ GetComponent ��� ���� �ֱ�(���� �ٲٴ� ����� ������ �ϴ� �̷���)
    public Animator m_Animator;

    private readonly int m_moveSpeedHashX = Animator.StringToHash("MoveDirectionX");
    private readonly int m_moveSpeedHashY = Animator.StringToHash("MoveDirectionY");

    public void SetParmBool(string name, bool value) => m_Animator.SetBool(name, value);
    public void SetParmTrigger(string name) => m_Animator.SetTrigger(name);
    public void SetParmFloat(string name, float value) => m_Animator.SetFloat(name, value);
    public void SetParmInt(string name, int value) => m_Animator.SetInteger(name, value);

    public float m_AnimationTime = 0f; // ���� �ִϸ��̼��� ����

    [HideInInspector]public bool m_IsJumpStart;
    private void Awake()
    {
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
        
    }
    public void UpdateMovementAnimation(Vector2 inputMovement)
    {
        m_Animator.SetFloat(m_moveSpeedHashX, inputMovement.x);
        m_Animator.SetFloat(m_moveSpeedHashY, inputMovement.y);
    }

    public void CrossFadeAnimation(string animationName, float transitionDuration = 0.25f)
    {
        m_Animator.CrossFade(animationName, transitionDuration);
    }


    //Enter���� ���� �ִϸ��̼��̸����� ������ ���̸� �����ϴ� ���� �߻�
    //�̸� Enter���� �񵿱� ó���� �غ������� �˵Ǽ� �ᱹ Update���� �ϱ��
    public float CheckComeInCurrentStateAni(string currentAniName)
    {
        AnimatorClipInfo[] clips;

        clips = m_Animator.GetCurrentAnimatorClipInfo(0);
        if (clips == null || clips[0].clip.name != currentAniName) return 0;
        return clips[0].clip.length;
    }

    public void CheckJumpUp()
    {
        m_IsJumpStart = true;
    }
}