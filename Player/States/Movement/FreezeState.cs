using Oblation.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem.Movement
{
    public class FreezeState : State
    {
        [SerializeField, Required]
        PlayerMovementController m_MovementController;

        public override void OnExit()
        {
            /* This happens, when player attack while fall. If player contact with the ground before freeze end,
             it needs to reset the jump count. */
            if (m_MovementController.IsTouchingGround())
                m_MovementController.m_JumpDataInstance.ResetJumpCount();
        }

    }
}
