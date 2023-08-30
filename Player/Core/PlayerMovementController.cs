using System;
using System.Collections;
using Oblation.ObstacleCheckers;
using Oblation.FSM;
using Oblation.PlayerInputSystem;
using Oblation.PlayerSystem.Attacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace Oblation.PlayerSystem.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {

        public float m_SavedYVelocity;
        #region Variables

        Rigidbody2D m_Rb;
        StateMachine m_FSM;

        WallChecker m_WallChecker;
        GroundDetection m_GroundDetection;

        bool m_IsMovementEnabled;
        bool m_HasJumpRequest;
        bool m_HasLandCooldown;
        bool m_HasPlatformPassCooldown;
        bool m_IsSlideKeyBeingHold;


        [Header("Player Settings")]
        [SerializeField] [Required]
        PlayerConfigurations m_Configurations;

        [Header("Dependencies")]
        [SerializeField] [Required]
        Collider2D m_PlayerCollision;


        [Header("States")]
        [SerializeField, Required] GroundState m_GroundState;
        [SerializeField, Required] JumpState m_JumpState;
        [SerializeField, Required] AirState m_AirState;
        [SerializeField, Required] LandState m_LandState;
        [SerializeField, Required] WallSlideState m_WallSlideState;
        [SerializeField, Required] FreezeState m_FreezeState;
        [SerializeField, Required] SlideState m_SlideState;
        [FormerlySerializedAs("m_OneWayPlatformState")] [SerializeField, Required]
        PassPlatformState m_PassPlatformState;

        PlayerAttackController m_AttackController;


        public JumpData m_JumpDataInstance { get; private set; }
        public WallSlideData WallSlideDataInstance { get; private set; }

        #endregion

        public void DisableMovement()
        {
            /* Triggering required to prevent bugs, because state change needs to be called
             instantly, rather than waiting Tick calls.*/
            m_FSM.TriggerState(m_FreezeState);

            m_IsMovementEnabled = false;
        }

        public void EnableMovement() => m_IsMovementEnabled = true;

        public void ClearJumpRequest()
        {
            m_HasJumpRequest = false;
            StopCoroutine(nameof(CreateJumpRequest));
        }

        public void StartLandCooldown() => StartCoroutine(LandCooldown(0.1f));
        
        void Update() => m_FSM.Tick();
        void FixedUpdate()
        {
            m_FSM.PhysicsTick();
            if (Mathf.Abs(m_Rb.velocity.y) >= .1f)
                m_SavedYVelocity = m_Rb.velocity.y;
        }
        void OnEnable()
        {
            PlayerInputs.e_OnJumpStarted += OnJumpStarted;
            PlayerInputs.e_OnPassPlatformPressed += OnPassPlatformKeyPressed;
            PlayerInputs.e_OnDiveStarted += OnSlideKeyPressed;
            PlayerInputs.e_OnDiveCancelled += OnSlideKeyReleased;
        }
        void OnSlideKeyPressed()
        {
            m_IsSlideKeyBeingHold = true;
        }
        void OnSlideKeyReleased()
        {
            m_IsSlideKeyBeingHold = false;
        }

        void OnDisable()
        {
            PlayerInputs.e_OnJumpStarted -= OnJumpStarted;
            PlayerInputs.e_OnPassPlatformPressed -= OnPassPlatformKeyPressed;
            PlayerInputs.e_OnDiveStarted -= OnSlideKeyPressed;
            PlayerInputs.e_OnDiveCancelled -= OnSlideKeyReleased;
        }
        void OnPassPlatformKeyPressed()
        {
            if (m_PassPlatformState.m_HasCooldown || m_PassPlatformState.m_CurrentPlatform == null)
                return;

            m_FSM.TriggerState(m_PassPlatformState);
        }
        void Awake()
        {
            m_Rb = GetComponent<Rigidbody2D>();
            m_AttackController = GetComponent<PlayerAttackController>();
            EnableMovement();
            SetupWallDetection();
            m_GroundDetection = new GroundDetection(m_PlayerCollision, 8, m_Configurations.ObstacleLayerMask);

            SetupStateMachine();
        }
        void SetupWallDetection() => m_WallChecker = new WallChecker(m_PlayerCollision, 2, m_Configurations.ObstacleLayerMask);
        public bool IsTouchingGround() => m_GroundDetection.IsTouchingGround() && !m_HasLandCooldown;
        bool IsTouchingToWall() => m_WallChecker.DetectWall(PlayerInputs.m_HorizontalMovement * Vector2.right);
        void OnJumpStarted() => StartCoroutine(CreateJumpRequest(0.12f));

        void SetupStateMachine()
        {
            m_JumpDataInstance = new JumpData(m_Configurations.JumpCount, m_Configurations.JumpForce, m_Rb);
            WallSlideDataInstance = new WallSlideData();

            m_FSM = new StateMachine();
            m_FSM.InitState(m_GroundState);

            m_FSM.AddAnyTransition(m_FreezeState, () => !m_IsMovementEnabled && m_FSM.m_GetCurrentState() != m_FreezeState);
            To(m_FreezeState, m_GroundState, () => m_IsMovementEnabled);
            To(m_FreezeState, m_GroundState, AttackCancellable());

            To(m_GroundState, m_JumpState, CanJump());
            To(m_GroundState, m_AirState, NoMoreContactToGround());
            To(m_GroundState, m_PassPlatformState, () => false);

            To(m_PassPlatformState, m_AirState, () => true);


            To(m_AirState, m_LandState, OnLand());
            To(m_AirState, m_JumpState, CanJump());
            To(m_AirState, m_WallSlideState, CanWallSlide());


            To(m_LandState, m_SlideState, CanSlide());
            To(m_LandState, m_GroundState, () => true);

            To(m_SlideState, m_GroundState, TooSlowToSlide());
            To(m_SlideState, m_JumpState, CanJump());
            
            To(m_WallSlideState, m_AirState, WhenJumpKeyTriggered());
            To(m_WallSlideState, m_AirState, NoContactToWall());

            To(m_JumpState, m_AirState, AlwaysTrue());
            return;

            void To(State from, State to, Func<bool> condition) => m_FSM.AddTransition(@from, to, condition);

            Func<bool> AttackCancellable() => () => Mathf.Abs(PlayerInputs.m_HorizontalMovement) > 0 && m_AttackController.m_AttackState.m_Cancellable;
            Func<bool> CanJump() => () => m_HasJumpRequest && m_JumpDataInstance.CanJump && m_IsMovementEnabled;
            Func<bool> WhenJumpKeyTriggered() => () => m_HasJumpRequest;
            Func<bool> OnLand() => IsTouchingGround;
            Func<bool> NoMoreContactToGround() => () => !IsTouchingGround();
            Func<bool> CanWallSlide() => () => IsTouchingToWall() && WallSlideDataInstance.HasCooldown == false && m_IsMovementEnabled;
            Func<bool> NoContactToWall() => () => !IsTouchingToWall();
            Func<bool> AlwaysTrue() => () => true;
            Func<bool> CanSlide() => () => m_IsSlideKeyBeingHold && Mathf.Abs(m_SavedYVelocity) > 5;
            Func<bool> TooSlowToSlide() => () => Mathf.Abs(m_Rb.velocity.x) < 2 || !m_IsSlideKeyBeingHold;
        }

        IEnumerator CreateJumpRequest(float duration)
        {
            if (m_HasJumpRequest) yield break;

            m_HasJumpRequest = true;
            yield return new WaitForSeconds(duration);
            m_HasJumpRequest = false;
        }

        IEnumerator LandCooldown(float duration)
        {
            if (m_HasLandCooldown) yield break;

            m_HasLandCooldown = true;
            yield return new WaitForSeconds(duration);
            m_HasLandCooldown = false;
        }


    }

}
