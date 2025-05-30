// MainState : ���� ���� ����
// SubFlags : ���� ���� ����
// Main + Sub

using System;
using System.Collections.Generic;
namespace DUS.Player.Locomotion
{
    public enum LocomotionMainState
    {
        Idle = 0,           // Idle
        Move = 1,           // �⺻ �̵�
        Jump = 2,          // ����
        InAir = 3,          // ���� (����/����)
        Land = 4,           // ����
        Dodge = 5,          // ������(ȸ�Ǳ�)
        Slide = 6,          // �����̵�
        Climb = 7,          // ���
        WallRun = 8,         // �� �޸���
        Staggered = 9,         // �ǰ� ���� ���� ����
        Knockback = 10,         // �˹�
    }

    /// <summary>
    /// Flags�� ������ ������ ������ ������ ���. ��, ���� ���°� ����
    /// Flags�� 2�� �������� 2�� ������ ������ ����Ͽ� �����ؾ���
    /// CombatIdleState =0, FalgButtonGroupManager = 1, Croucning = 2, 4, 8 �̷��� ���ٴ� ����Ʈ����)
    /// </summary>
    [Flags]
    public enum LocomotionSubFlags
    {
        None = 0,
        Run = 1 << 1,           // �޸���
        Crouch = 1 << 2,        // �ɱ�
        CrouchRun = 1 << 3      // �ɾƼ� �޸��� 
    }

    public class LocomotionStateUtility
    {
        #region ======================================== MainState ����
        public Dictionary<LocomotionMainState, LocomotionStrategyState> m_MainStrategyMap { get; private set; } = new();
        //SetBool = 0�� �ε���, SetTrigger = 1��
        public Dictionary<LocomotionMainState, string[]> m_MainStateAniParmMap { get; private set; } = new()
        {
            { LocomotionMainState.Idle, new string[]{"IsIdle" } },
            { LocomotionMainState.Move, new string[]{"IsMove" } },
            { LocomotionMainState.Jump, new string[]{"IsJump","Jump"} },
            { LocomotionMainState.InAir, new string[]{"IsInAir" } },
            { LocomotionMainState.Land, new string[]{"IsLand" } },
            { LocomotionMainState.Slide, new string[]{"IsSlide", "Slide" } },
            { LocomotionMainState.Climb, new string[]{"IsClimb" } },
            { LocomotionMainState.WallRun, new string[]{"IsWallRun" } }
        };
        public void InitializeCreateMainStateMap(PlayerCore player)
        {
            m_MainStrategyMap[LocomotionMainState.Idle] = new IdleState(player);
            m_MainStrategyMap[LocomotionMainState.Move] = new MoveState(player);
            m_MainStrategyMap[LocomotionMainState.Jump] = new JumpState(player);
            m_MainStrategyMap[LocomotionMainState.InAir] = new InAirState(player);
            m_MainStrategyMap[LocomotionMainState.Land] = new LandState(player);
            m_MainStrategyMap[LocomotionMainState.Slide] = new SlideState(player);
            m_MainStrategyMap[LocomotionMainState.Climb] = new ClimbState(player);
            m_MainStrategyMap[LocomotionMainState.WallRun] = new WallRunState(player);
        }
        
        #endregion ======================================== /MainSate ����

        #region ======================================== SubFlags ����
        private HashSet<LocomotionSubFlags> m_CurrentFlagsHash = new();
        public Dictionary<LocomotionSubFlags, string> m_FlagAniMap { get; private set; } = new()
        {
            { LocomotionSubFlags.None, "IsNone" },
            { LocomotionSubFlags.Run, "IsRun" },
            { LocomotionSubFlags.Crouch, "IsCrouch" },
            { LocomotionSubFlags.CrouchRun, "IsCrouchRun" },
        };

        //HashSet�� Add �ߺ� �ڵ� ����
        /// Flag + Ani ��� ����
        public void SetLocomotionFlag(LocomotionSubFlags flag) => m_CurrentFlagsHash.Add(flag);
        public void RemoveLocomotionFlag(LocomotionSubFlags flag) => m_CurrentFlagsHash.Remove(flag);
        public bool HasLocomotionFlag(LocomotionSubFlags flag) => m_CurrentFlagsHash.Contains(flag);
        public void AllClearFlags() => m_CurrentFlagsHash.Clear();
        
        #endregion ======================================== /SubFlags ����
    }
}
