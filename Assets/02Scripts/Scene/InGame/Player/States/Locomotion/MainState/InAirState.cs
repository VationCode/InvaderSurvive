using DUS.Player.Locomotion;

public class InAirState : LocomotionStrategyState
{
    public InAirState(PlayerCore playerCore) : base(playerCore) { }
    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.InAir;

    protected override AniParmType[] SetAniParmType() => new AniParmType[] { AniParmType.SetBool };
    protected override float SetMoveSpeed() => 0;

    public override void Enter()
    {
        base.Enter();
        //TODO : �ִϸ��̼ǿ��� Trigger ������ ��� �������� ���� �� ���߻��µ� �ֱ⿡ ���� ���� ���� �ʿ�
        //�ϴ��� ���� ���¿����� ���߸� ����
        m_IsNotInputMove = true;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        m_Locomotion.HandleMaintainForwardForceMove(m_Locomotion.m_CurrentVelocityXZ * m_PlayerCore.m_JumpForwardSpeedPercent);
    }
    public override void Update()
    {
        base.Update();
        if(m_Locomotion.m_IsGrounded)
        {
            m_Locomotion.SetNextState(LocomotionMainState.Land);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}