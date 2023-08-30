using Oblation.SenseEngine;
using UnityEngine;
using Oblation.FSM;
using Oblation.PlayerInputSystem;
using Sirenix.OdinInspector;
using Spine.Unity;


namespace Oblation.PlayerSystem.Movement
{
    public class ShiftState : State
    {
        #region Variables

        [SerializeField, Required] Rigidbody2D m_Rb;

        [SerializeField, Required] PlayerConfigurations m_Configurations;

        [SerializeField, Required] SkeletonAnimation m_SpineComponent;

        [SerializeField] SenseEnginePlayer m_sepEnterShift;
        [SerializeField] SenseEnginePlayer m_sepLeaveShift;

        float m_DefaultGravityScale;

        #endregion

        #region Monobehaviours

        void Awake() => m_DefaultGravityScale = m_Rb.gravityScale;

        #endregion

        #region State Callbacks

        public override void PhysicsTick()
        {
            base.PhysicsTick();
            MoveForward();
            SetAngle();
            m_SpineComponent.AnimationState.SetAnimation(1, "dash", true);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            SetAngleInstant();
            m_Rb.gravityScale = 0;
            m_Rb.freezeRotation = false;
            m_sepEnterShift.PlayIfExist();
        }

        public override void OnExit()
        {
            base.OnExit();
            m_Rb.gravityScale = m_DefaultGravityScale;
            m_Rb.rotation = 0;
            m_Rb.freezeRotation = true;
            m_sepLeaveShift.PlayIfExist();
            GiveSpeedBoost(1.5f);
        }

        void GiveSpeedBoost(float boostValue) => m_Rb.velocity *= boostValue;

        #endregion

        #region Local Methods

        void MoveForward()
        {
            m_Rb.velocity = m_Rb.transform.right * (m_Configurations.ShiftModeSpeed * Time.fixedDeltaTime);
        }

        void SetAngle()
        {
            if (Camera.main is null) return;
            var angle = m_Rb.position.Angle(Camera.main.ScreenToWorldPoint(PlayerInputs.m_MousePosition));
            m_Rb.rotation = Mathf.MoveTowardsAngle(m_Rb.rotation, angle, Time.fixedDeltaTime * m_Configurations.ShiftModeTurnSpeed);
        }

        void SetAngleInstant()
        {
            if (Camera.main is null) return;
            var angle = m_Rb.position.Angle(Camera.main.ScreenToWorldPoint(PlayerInputs.m_MousePosition));
            m_Rb.rotation = angle;
        }

        #endregion
    }
}
