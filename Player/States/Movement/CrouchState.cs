using Oblation.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem.Movement
{
    public class CrouchState : State
    {
        [SerializeField, Required]
        Rigidbody2D m_Rb;

        [SerializeField, Min(0)]
        float m_FrictionAcceleration = 40f;
        void Reset()
        {
            m_Rb = transform.root.GetComponent<Rigidbody2D>();
        }

        public override void PhysicsTick()
        {
            SlowDown();
        }

        void SlowDown()
        {
            var currentXVelocity = m_Rb.velocity.x;
            currentXVelocity = Mathf.MoveTowards(currentXVelocity, 0, Time.fixedDeltaTime * m_FrictionAcceleration);
            m_Rb.velocity = new Vector2(currentXVelocity, m_Rb.velocity.y);
        }

    }
}
