using UnityEngine;

namespace DUS.Player.Combat {
    public class PlayerCombat
    {
        private PlayerCore m_playerCore;
        private Animator m_animator;
        private CombatStateUtility m_stateUtility;

        public PlayerCombat(PlayerCore playerCore)
        {
            m_playerCore = playerCore;
            m_animator = m_playerCore.m_AnimationManager.m_Animator;
        }

        #region ======================================== State ����
        private CombatStrategyState m_currentStrategyState;
        public CombatStrategyState m_nextStrategyState { get; set; }
        public CombatStrategyState m_prevStrategyState { get; private set; }
        #endregion ======================================== /State ����
        //PlayerCore Start���� ȣ��
        public void InitializeCombatStart()
        {
            m_currentStrategyState = m_stateUtility.m_MainStrategyMap[CombatMainState.CombatIdle];
            m_currentStrategyState.Enter();
        }

        public void FixedUpdate()
        {

        }

        public void Update()
        {

            m_currentStrategyState?.Update();

            if (m_currentStrategyState != m_nextStrategyState)
                UpdateSwitchState(m_nextStrategyState);
        }
        public void LateUpdate(){}
        public void SetNextState(CombatMainState combatMainState)
        {
            m_nextStrategyState = m_stateUtility.m_MainStrategyMap[combatMainState];
        }

        public void UpdateSwitchState(CombatStrategyState nextState)
        {
            if (nextState == null) return;
            m_currentStrategyState?.Exit();
            m_currentStrategyState = nextState;
            m_currentStrategyState?.Enter();
        }


    }
}