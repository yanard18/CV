using System;
using Oblation.SenseEngine;
using Oblation.FSM;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oblation.PlayerSystem.Movement
{
    public class LandState : State
    {
        [FormerlySerializedAs("m_Controller")] [SerializeField, Required] PlayerMovementController m_MovementController;
        [SerializeField] SenseEnginePlayer m_sepLand;

        #region State Callbacks

        public override void OnEnter()
        {
            base.OnEnter();
            m_MovementController.m_SavedYVelocity = 0;
            m_MovementController.m_JumpDataInstance.ResetJumpCount();
            m_sepLand.PlayIfExist();
        }

        #endregion
        
    }
}
