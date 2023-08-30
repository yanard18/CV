using System;
using Oblation.FSM;
using Oblation.PlayerInputSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem.Movement
{
    public class GroundState : State
    {
        StateMachine m_FSM;

        [SerializeField, Required]
        MoveState m_MoveState;

        [SerializeField, Required]
        IdleState m_IdleState;

        [SerializeField, Required]
        CrouchState m_CrouchState;

        [SerializeField, Required]
        GroundExitState m_ExitState;

        [SerializeField, Required]
        PlayerMovementController m_Controller;

        bool m_IsDownKeyBeingHold;

        void OnEnable()
        {
            PlayerInputs.e_OnDiveStarted += OnDownKeyPressed;
            PlayerInputs.e_OnDiveCancelled += OnDownKeyReleased;
        }


        void OnDisable()
        {
            PlayerInputs.e_OnDiveStarted -= OnDownKeyPressed;
            PlayerInputs.e_OnDiveCancelled -= OnDownKeyReleased;
        }

        void OnDownKeyPressed()
        {
            m_IsDownKeyBeingHold = true;
        }
        void OnDownKeyReleased()
        {
            m_IsDownKeyBeingHold = false;
        }


        void Awake()
        {
            m_FSM = new StateMachine();
            m_FSM.InitState(m_IdleState);

            To(m_IdleState, m_MoveState, HasMovementInput());
            To(m_IdleState, m_CrouchState, () => m_IsDownKeyBeingHold);

            To(m_CrouchState, m_IdleState, () => !m_IsDownKeyBeingHold);

            To(m_MoveState, m_IdleState, HasNotMovementInput());

            To(m_ExitState, m_IdleState, HasNotMovementInput());
            To(m_ExitState, m_MoveState, HasMovementInput());
            m_FSM.AddAnyTransition(m_ExitState, () => false);
            return;

            void To(State from, State to, Func<bool> condition) => m_FSM.AddTransition(@from, to, condition);

            Func<bool> HasMovementInput() => () => Mathf.Abs(PlayerInputs.m_HorizontalMovement) > 0;
            Func<bool> HasNotMovementInput() => () => PlayerInputs.m_HorizontalMovement == 0;
        }

        public override void OnEnter()
        {
            m_Controller.EnableMovement();
        }

        public override void OnExit()
        {
            m_FSM.TriggerState(m_ExitState);
        }

        public override void Tick()
        {
            m_FSM.Tick();
        }
        public override void PhysicsTick()
        {
            m_FSM.PhysicsTick();
        }
    }
}
