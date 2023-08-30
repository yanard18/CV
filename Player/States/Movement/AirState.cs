using UnityEngine;
using Oblation.FSM;
using Oblation.PlayerInputSystem;
using Oblation.Movement;
using PixelMotion;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;

namespace Oblation.PlayerSystem.Movement
{
    public class AirState : State
    {
        [SerializeField, Required] PlayerConfigurations m_Configurations;
        [SerializeField, Required] Rigidbody2D m_Rb;
        [SerializeField, Required] SkeletonAnimation m_SkeletonComponent;

        IMove m_IHorizontalMovement;
        IMove m_IVerticalMovement;
        bool m_DiveKeyPressed;



        #region Monobehaviour

        void Awake()
        {
            m_IHorizontalMovement = new HorizontalPhysicMovement(
                m_Rb,
                m_Configurations.AirStrafeMaxXVelocity,
                m_Configurations.AirStrafeXAcceleration
            );

            m_IVerticalMovement = new VerticalPhysicMovement(
                m_Rb,
                m_Configurations.AirStrafeMaxYVelocity,
                m_Configurations.AirStrafeYAcceleration
            );


        }

        #endregion

        #region State Callbacks

        public override void OnEnter()
        {
            base.OnEnter();
            PlayerInputs.e_OnDiveStarted += OnDiveStarted;
            PlayerInputs.e_OnDiveCancelled += OnDiveCancelled;
        }

        public override void OnExit()
        {
            PlayerInputs.e_OnDiveStarted -= OnDiveStarted;
            PlayerInputs.e_OnDiveCancelled -= OnDiveCancelled;
            m_DiveKeyPressed = false;
        }

        public override void PhysicsTick()
        {
            base.PhysicsTick();

            HandleAirStrafe();
            HandleDive();
        }

        public override void Tick()
        {
            if (m_Rb.velocity.y < 0)
            {
                if (m_SkeletonComponent.AnimationState.GetCurrent(0).ToString() != "fall")
                    m_SkeletonComponent.AnimationState.SetAnimation(0, "fall", false);
            }
        }

        #endregion
        void HandleAirStrafe()
        {
            var moveDirection = PlayerInputs.m_HorizontalMovement * Vector2.right;
            if (moveDirection.magnitude != 0)
                m_SkeletonComponent.skeleton.FlipX = moveDirection.x < 0;
            m_IHorizontalMovement.Move(moveDirection);
        }

        void HandleDive()
        {
            if (PressedToDiveKey())
                m_IVerticalMovement.Move(Vector2.down);
        }

        bool PressedToDiveKey() => m_DiveKeyPressed;

        void OnDiveStarted() => m_DiveKeyPressed = true;
        void OnDiveCancelled() => m_DiveKeyPressed = false;

    }
}
