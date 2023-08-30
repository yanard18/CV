using System;
using System.Collections;
using Oblation.FSM;
using Oblation.PlayerInputSystem;
using Oblation.PlayerSystem.Movement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem.Attacks
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField, Required]
        public AttackState m_AttackState;

        [SerializeField, Required]
        IdleState m_IdleState;


        [SerializeField, Required]
        BloodCastState m_BloodCastState;


        StateMachine m_FSM;
        bool m_HasAttackRequest;
        bool m_BloodCastKeyBeingHold;

        bool m_CanAttack;


        public void EnableAttack()
        {
            m_CanAttack = true;
        }
        public void DisableAttack()
        {
            m_CanAttack = false;
        }

        void Update() => m_FSM.Tick();
        void FixedUpdate() => m_FSM.PhysicsTick();

        void OnEnable()
        {
            PlayerInputs.e_OnAttack1Started += OnAttackKeyPressed;
            PlayerInputs.e_OnBloodCastingPressed += OnBloodCastKeyPressed;
            PlayerInputs.e_OnBloodCastingReleased += OnBloodCastKeyReleased;
        }
        void OnDisable()
        {
            PlayerInputs.e_OnAttack1Started -= OnAttackKeyPressed;
            PlayerInputs.e_OnBloodCastingPressed -= OnBloodCastKeyPressed;
            PlayerInputs.e_OnBloodCastingReleased -= OnBloodCastKeyReleased;
        }

        void OnAttackKeyPressed()
        {
            StartCoroutine(CreateAttackRequest(.025f));
        }

        void OnBloodCastKeyPressed()
        {
            m_BloodCastKeyBeingHold = true;
        }
        void OnBloodCastKeyReleased()
        {
            m_BloodCastKeyBeingHold = false;
        }

        void Awake()
        {
            EnableAttack();
            m_FSM = new StateMachine();
            m_FSM.InitState(m_IdleState);

            To(m_IdleState, m_AttackState, () => m_HasAttackRequest && m_CanAttack);
            To(m_IdleState, m_BloodCastState, CanUseBloodCast());
            
            To(m_AttackState, m_IdleState, () => m_AttackState.m_HasFinished);
            To(m_AttackState, m_AttackState, () => m_AttackState.m_Cancellable && m_HasAttackRequest);

            To(m_BloodCastState, m_IdleState, () => m_BloodCastState.m_IsCompleted);
            return;

            void To(State from, State to, Func<bool> condition) => m_FSM.AddTransition(@from, to, condition);
            Func<bool> CanUseBloodCast() => () => m_BloodCastKeyBeingHold && Player.s_Instance.m_BloodCastSlot.CanUseBloodCast() && m_CanAttack;
        }

        IEnumerator CreateAttackRequest(float duration)
        {
            if (m_HasAttackRequest) yield break;

            m_HasAttackRequest = true;
            yield return new WaitForSeconds(duration);
            m_HasAttackRequest = false;
        }

    }
}
