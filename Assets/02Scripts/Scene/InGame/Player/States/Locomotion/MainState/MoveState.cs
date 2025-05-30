using DUS.Player.Locomotion;

public class MoveState : LocomotionStrategyState
{
    public MoveState(PlayerCore playerCore) : base(playerCore) { }

    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.Move;
    protected override AniParmType[] SetAniParmType() => new AniParmType[] { AniParmType.SetBool };
    protected override float SetMoveSpeed() => m_PlayerCore.m_WalkSpeed;
    public override void Enter()
    {
        base.Enter();
    }
    public override void FixedUpdate()
    {
        // 1. UpdateMovement ����
        base.FixedUpdate();
    }
    public override void Update()
    {
        // �ִϸ��̼� �� ��ȯ ���� ����
        base.Update();

        //Flags && Speed ����
        bool isRun = m_PlayerCore.m_InputManager.m_IsRun_LocoF;
        bool isCrouch = m_PlayerCore.m_InputManager.m_IsCrouch_LocoF;
        bool isMove = m_PlayerCore.m_InputManager.m_IsMove_LocoM; // �̵� �Է� ����
        bool isJump = m_PlayerCore.m_InputManager.m_IsJump_LocoM;

        // 3. 5�� �Ǳ����� ���� SubFlag ���� üũ
        bool hasRun = m_Locomotion.m_StateUtility.HasLocomotionFlag(LocomotionSubFlags.Run);
        bool hasCrouch = m_Locomotion.m_StateUtility.HasLocomotionFlag(LocomotionSubFlags.Crouch);
        bool hasCrouchRun = m_Locomotion.m_StateUtility.HasLocomotionFlag(LocomotionSubFlags.CrouchRun);

        // 4. �� ���� ���� 
        if (!hasCrouch && hasRun && isCrouch)             // �޸��� �� �ɱ� = �����̵�
        {
            // �����̵� ����
            m_Locomotion.SetNextState(LocomotionMainState.Slide);
            m_Locomotion.HandleCheckFlags(LocomotionSubFlags.Crouch, isCrouch);
            return;
        }

        if (isRun && isCrouch)       // �ɱ� �� �޸��� = ������ �޸���
        {
            m_moveSpeed = m_PlayerCore.m_CrouchRunSpeed;
            hasCrouchRun = true;
        }
        else if (isRun)          // �Ϲ� �޸���
        {
            m_moveSpeed = m_PlayerCore.m_RunSpeed;
            hasCrouchRun = false;
        }
        else if (isCrouch)       // �Ϲ� �ɱ�
        {
            m_moveSpeed = m_PlayerCore.m_CrouchSpeed;
            hasCrouchRun = false;
        }
        else
        {
            m_moveSpeed = m_PlayerCore.m_WalkSpeed;
            hasCrouchRun = false;
        }

        // 5. SubFlag ������Ʈ
        m_Locomotion.HandleCheckFlags(LocomotionSubFlags.CrouchRun, hasCrouchRun);
        m_Locomotion.HandleCheckFlags(LocomotionSubFlags.Run, isRun);
        m_Locomotion.HandleCheckFlags(LocomotionSubFlags.Crouch, isCrouch);


        // ChangeMain
        if (isJump)
        {
            m_Locomotion.SetNextState(LocomotionMainState.Jump);
        }
        else if (!isMove)
        {
            m_Locomotion.SetNextState(LocomotionMainState.Idle);
        }
    }
    public override void Exit()
    {
        base.Exit();
        m_Locomotion.AllClearFlag();
    }
}