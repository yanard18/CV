using Oblation.FSM;
using Oblation.Movement;
using Oblation.PlayerInputSystem;
using UnityEngine;

namespace Oblation.PlayerSystem
{
    public class BloodCastState : State
    {
        public bool m_IsCompleted;
        
        Rigidbody2D m_Rb;
        IMove m_Movement;
        bool m_IsStateActive;
        bool m_IsCastKeyBeingHold;


        public override void OnEnter()
        {
            m_IsStateActive = true;
            m_IsCompleted = false;
            
            Player.s_Instance.m_BloodCastSlot.m_ActiveCast.e_OnCastCompleted += OnCastCompleted;
            Player.s_Instance.m_BloodCastSlot.UseActiveBloodCast();

            // Stop the player.
            m_Movement.Move(Vector2.zero);
        }

        public override void Tick()
        {
            if (m_IsCastKeyBeingHold)
                Player.s_Instance.m_BloodCastSlot.m_ActiveCast.Hold();
        }

        public override void OnExit()
        {
            m_IsStateActive = false;
            m_IsCompleted = false;
            Player.s_Instance.m_BloodCastSlot.SwitchToNextCast();
        }
        void OnEnable()
        {
            PlayerInputs.e_OnBloodCastingPressed += OnBloodCastKeyPressed;
            PlayerInputs.e_OnBloodCastingReleased += OnBloodCastKeyReleased;
        }
        void OnDisable()
        {
            PlayerInputs.e_OnBloodCastingPressed -= OnBloodCastKeyPressed;
            PlayerInputs.e_OnBloodCastingReleased -= OnBloodCastKeyReleased;

        }
        void Awake()
        {
            m_Rb = transform.root.GetComponent<Rigidbody2D>();
            m_Movement = new HorizontalInstantMovement(m_Rb, 0);
        }

        void OnBloodCastKeyPressed()
        {
            m_IsCastKeyBeingHold = true;
        }

        void OnBloodCastKeyReleased()
        {
            m_IsCastKeyBeingHold = false;

            if (m_IsStateActive)
                Player.s_Instance.m_BloodCastSlot.m_ActiveCast.Release();
        }

        void OnCastCompleted()
        {
            m_IsCompleted = true;
            Player.s_Instance.m_BloodCastSlot.m_ActiveCast.e_OnCastCompleted -= OnCastCompleted;
        }

    }
}
