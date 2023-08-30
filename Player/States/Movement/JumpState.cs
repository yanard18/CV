using System.Collections;
using Oblation.SenseEngine;
using UnityEngine;
using Oblation.FSM;
using PixelMotion;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine.Serialization;

namespace Oblation.PlayerSystem.Movement
{
    public class JumpState : State
    {

        #region Variables

        [FormerlySerializedAs("m_Controller")] [SerializeField, Required] PlayerMovementController m_MovementController;

        [SerializeField] SenseEnginePlayer m_sepJump;

        [SerializeField] SkeletonAnimation m_spineComponent;
        


        #endregion

        #region State Callbacks

        public override void OnEnter()
        {
            base.OnEnter();
            Jump();
            m_MovementController.StartLandCooldown();
            var jumpAnim = m_spineComponent.AnimationState.SetAnimation(0, "jump", false);
            jumpAnim.MixDuration = 0.05f;



        }

        #endregion

        #region Local Methods

        void Jump()
        {
            var jumpData = m_MovementController.m_JumpDataInstance;
            jumpData.m_Rb.velocity = new Vector2(jumpData.m_Rb.velocity.x, jumpData.m_JumpForce);
            jumpData.m_nAvailableJump--;
            m_sepJump.PlayIfExist();
            m_MovementController.StartCoroutine(StartJumpCooldown(0.15f));
        }

        IEnumerator StartJumpCooldown(float duration)
        {
            var jumpData = m_MovementController.m_JumpDataInstance;
            jumpData.m_HasCooldown = true;
            yield return new WaitForSeconds(duration);
            jumpData.m_HasCooldown = false;
        }

        #endregion
    }

    public class JumpData
    {
        readonly int m_nMaxJump;

        public readonly float m_JumpForce;
        public readonly Rigidbody2D m_Rb;

        public int m_nAvailableJump;
        public bool m_HasCooldown;


        public bool CanJump => m_nAvailableJump > 0 && m_HasCooldown == false;

        public void ResetJumpCount() => m_nAvailableJump = m_nMaxJump;

        public JumpData(int nMaxJump, float jumpForce, Rigidbody2D rb)
        {
            m_nMaxJump = nMaxJump;
            m_nAvailableJump = m_nMaxJump;
            m_JumpForce = jumpForce;
            m_Rb = rb;
        }

    }
}
