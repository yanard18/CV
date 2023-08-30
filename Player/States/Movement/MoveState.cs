using Oblation.FSM;
using Oblation.PlayerInputSystem;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace Oblation.PlayerSystem.Movement
{
    public class MoveState : State
    {
        #region Variables

        [SerializeField, Required] PlayerConfigurations m_Configurations;

        [SerializeField, Required] Rigidbody2D m_Rb;


        [SerializeField, Required] SkeletonAnimation m_skeletonComponent;


        float m_XVelocity;

        #endregion

        #region State Callbacks

        public override void PhysicsTick() => Move();

        public override void OnEnter()
        {
            base.OnEnter();
            m_XVelocity = m_Rb.velocity.x;
            m_skeletonComponent.AnimationState.TimeScale = 1.4f;
            m_skeletonComponent.AnimationState.SetAnimation(0, "run", true);
        }

        #endregion

        #region Local Methods

        void Move()
        {
            var movementDirection = Mathf.Sign(PlayerInputs.m_HorizontalMovement);
            m_skeletonComponent.skeleton.FlipX = movementDirection < 0;


            m_XVelocity = Mathf.MoveTowards(
                m_XVelocity,
                m_Configurations.m_DesiredMovementVelocity * movementDirection,
                Time.fixedDeltaTime * m_Configurations.MovementAcceleration);

            m_Rb.velocity = new Vector2(m_XVelocity, m_Rb.velocity.y);
        }

        #endregion

    }
}
