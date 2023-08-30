using Oblation.FSM;
using UnityEngine;

namespace Oblation.PlayerSystem.Attacks
{
    public class PlayerAttackSliceState : State
    {
        [SerializeField] Rigidbody2D m_Rb;

        const float DASH_SPEED = 100.0f;
        Vector2 m_MovementDir;


        #region State Callbacks

        public override void OnEnter()
        {
            m_MovementDir = m_Rb.velocity.normalized;
            m_Rb.velocity = m_MovementDir * DASH_SPEED;
        }

        #endregion
        

    }
}