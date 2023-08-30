using System.Collections;
using Oblation.FSM;
using Oblation.PlayerSystem.Attacks;
using PixelMotion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oblation.PlayerSystem.Movement
{
    public class WallSlideState : State
    {
        [SerializeField, Required] Rigidbody2D m_Rb;

        [FormerlySerializedAs("m_Controller")] [SerializeField, Required]
        PlayerMovementController m_MovementController;

        [SerializeField, Required] 
        Collider2D m_Collider;

        [SerializeField, Required] 
        PlayerConfigurations m_Configurations;

        [SerializeField, Required]
        PlayerAttackController m_AttackController;


        float m_DefaultGravityScale;

        void Awake() => m_DefaultGravityScale = m_Rb.gravityScale;


        #region Callback States

        public override void OnEnter()
        {
            base.OnEnter();
            m_AttackController.DisableAttack();
            m_Rb.gravityScale = m_Configurations.WallSlideGravity;
            m_Rb.velocity /= 4.0f;
            m_MovementController.m_JumpDataInstance.ResetJumpCount();
            m_MovementController.ClearJumpRequest();
        }

        public override void OnExit()
        {
            base.OnExit();

            m_Rb.gravityScale = m_DefaultGravityScale;
            Vector2? hitNormal = FindWallContactNormal();

            if (hitNormal == null)
                return;

            ExecuteJump(hitNormal);
            m_AttackController.EnableAttack();
        }

        #endregion

        #region Local Methods

        void ExecuteJump(Vector2? hitNormal)
        {

            if (hitNormal != null)
            {
                var horizontal = m_Configurations.WallSlideHorizontalBouncePower;
                var vertical = m_Configurations.WallSlideVerticalBouncePower;
                m_Rb.velocity = (hitNormal.Value * horizontal) + (Vector2.up * vertical);
            }

            m_MovementController.StartCoroutine(m_MovementController.WallSlideDataInstance.StartCooldown(0.12f));
        }

        Vector2? FindWallContactNormal()
        {
            const int horizontalRayCount = 2;
            var bounds = m_Collider.bounds;
            var horizontalRaySpacing = bounds.size.y;


            var bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            var bottomRight = new Vector2(bounds.max.x, bounds.min.y);

            for (var i = 0; i < horizontalRayCount; i++)
            {
                var rayDirection = Vector2.left;
                var hit = Physics2D.Raycast(bottomLeft, rayDirection, 0.1f, m_Configurations.ObstacleLayerMask);
                Debug.DrawRay(bottomLeft, Vector2.left * 5, Color.red, 3);

                if (hit)
                    return hit.normal;
            }

            for (var i = 0; i < horizontalRayCount; i++)
            {
                var rayStartPos = bottomRight + Vector2.up * (i * horizontalRaySpacing);
                var rayDirection = Vector2.right;

                var hit = Physics2D.Raycast(rayStartPos, rayDirection, 0.1f, m_Configurations.ObstacleLayerMask);
                Debug.DrawRay(rayStartPos, Vector2.right * 5, Color.red, 3);

                if (hit)
                    return hit.normal;
            }

            return null;
        }

        #endregion

    }

    public class WallSlideData
    {
        public bool HasCooldown;

        public IEnumerator StartCooldown(float duration)
        {
            HasCooldown = true;
            yield return new WaitForSeconds(duration);
            HasCooldown = false;
        }
    }
}
