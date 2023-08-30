using UnityEngine;
using Oblation.FSM;
using Sirenix.OdinInspector;
using Spine.Unity;


namespace Oblation.PlayerSystem.Movement
{
    public class IdleState : State
    {
        #region Variables

        [SerializeField, Required] Rigidbody2D m_Rb;
        [SerializeField] float m_FrictionAcceleration = 40.0f;
        [SerializeField, Required] SkeletonAnimation m_SkeletonAnimation;
        

        #endregion


        #region State Callbacks

        public override void OnEnter()
        {
            base.OnEnter();
            m_SkeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        }
        public override void PhysicsTick() => SlowDown();

        #endregion
        void SlowDown()
        {
            var currentXVelocity = m_Rb.velocity.x;
            currentXVelocity = Mathf.MoveTowards(currentXVelocity, 0, Time.fixedDeltaTime * m_FrictionAcceleration);
            m_Rb.velocity = new Vector2(currentXVelocity, m_Rb.velocity.y);
        }


    }
}
