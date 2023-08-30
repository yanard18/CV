using System;
using Oblation.FSM;
using Oblation.PlayerSystem.Movement;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;
using Event = Spine.Event;

namespace Oblation.PlayerSystem.Attacks
{
    public class AttackState : State
    {
        public bool m_HasFinished { get; private set; }
        public bool m_Cancellable { get; private set; }

        [SerializeField, Required] SkeletonAnimation m_SkeletonComponent;
        [SerializeField, Required] PlayerMovementController m_MovementController;

        void OnAnimationEvent(TrackEntry trackentry, Event e)
        {
            switch (e.Data.Name)
            {
                case "Attack Can Break":
                    m_Cancellable = true;
                    break;
                case "Attack Can Charge":
                    break;
                case "Attack Deal Damage":
                    break;
            }
        }
        public override void OnEnter()
        {
            ResetParams();
            m_SkeletonComponent.AnimationState.Event += OnAnimationEvent;
            m_SkeletonComponent.AnimationState.Complete += CompleteState;
            m_MovementController.DisableMovement();

            if (m_MovementController.IsTouchingGround())
            {
                Player.s_Instance.m_WeaponSlot.m_CurrentWeapon.Attack();
            }
            else
            {
                Player.s_Instance.m_WeaponSlot.m_CurrentWeapon.AirAttack();
            }
        }
        void CompleteState(TrackEntry trackentry)
        {
            if(trackentry.TrackIndex != 0)
                return;
            
            m_HasFinished = true;
        }

        public override void OnExit()
        {
            m_MovementController.EnableMovement();
            m_SkeletonComponent.AnimationState.Event -= OnAnimationEvent;
            m_SkeletonComponent.AnimationState.Complete -= CompleteState;
            ResetParams();

        }

        void ResetParams()
        {
            m_HasFinished = false;
            m_Cancellable = false;

        }



    }
}
