using Oblation.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem.Movement
{
    public class TeleportState : State
    {
        #region Variables

        [SerializeField, Required] Rigidbody2D m_Rb;

        [SerializeField, Required] PlayerConfigurations m_Configurations;

        public bool m_HasFinished;

        #endregion

        #region State Callbacks

        public override void OnEnter()
        {
            base.OnEnter();
            var velocityDirection = m_Rb.velocity.normalized;
            var desiredPosition = m_Rb.position + velocityDirection * m_Configurations.SliceTeleportDistance;

            if (IsThereObstacleBetweenPositions(m_Rb.position, desiredPosition))
                m_Rb.MovePosition(desiredPosition);
            else
                m_Rb.position = desiredPosition;

            m_HasFinished = true;
        }

        bool IsThereObstacleBetweenPositions(Vector2 startPos, Vector2 endPos)
        {
            var dir = startPos.Direction(endPos);
            var hit = Physics2D.Raycast(startPos, dir.normalized, dir.magnitude, m_Configurations.ObstacleLayerMask);
            return hit;
        }

        public override void OnExit()
        {
            m_Rb.velocity /= m_Configurations.SliceSpeedReductionAfterTeleport;
            m_HasFinished = false;
        }

        #endregion

    }
}
