using Oblation.Projectiles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem
{
    [CreateAssetMenu(menuName = "Player/Player Configuration")]
    public class PlayerConfigurations : ScriptableObject
    {
        [SerializeField, TabGroup("Movement")] float m_pDesiredMovementVelocity = 17.0f;
        public float m_DesiredMovementVelocity => m_pDesiredMovementVelocity;
        
        [SerializeField, TabGroup("Movement")] float _movementAcceleration = 80.0f;
        public float MovementAcceleration => _movementAcceleration;

        [SerializeField, TabGroup("Jump")] float _jumpDelayDuration = 0.1f;
        public float JumpDelayDuration => _jumpDelayDuration;

        [SerializeField, TabGroup("Jump")] float _jumpForce = 10.0f;
        public float JumpForce => _jumpForce;

        [SerializeField, TabGroup("Jump")] int _jumpCount;
        public int JumpCount => _jumpCount;

        [SerializeField, TabGroup("Wall Slide")]
        float _wallSlideGravity = 0.5f;
        public float WallSlideGravity => _wallSlideGravity;

        [SerializeField, TabGroup("Wall Slide")]
        float _wallSlideHorizontalBouncePower = 17.0f;
        public float WallSlideHorizontalBouncePower => _wallSlideHorizontalBouncePower;

        [SerializeField, TabGroup("Wall Slide")]
        float _wallSlideVerticalBouncePower = 20.0f;
        public float WallSlideVerticalBouncePower => _wallSlideVerticalBouncePower;

        
        [SerializeField, TabGroup("Air Strafe")]
        float _airStrafeXAcceleration = 100.0f;
        public float AirStrafeXAcceleration => _airStrafeXAcceleration;

        [SerializeField, TabGroup("Air Strafe")]
        float _airStrafeMaxXVelocity = 20.0f;
        public float AirStrafeMaxXVelocity => _airStrafeMaxXVelocity;

        [SerializeField, TabGroup("Air Strafe")]
        float _airStrafeYAcceleration = -200.0f;
        public float AirStrafeYAcceleration => _airStrafeYAcceleration;

        [SerializeField, TabGroup("Air Strafe")]
        float _airStrafeMaxYVelocity = 25.0f;
        public float AirStrafeMaxYVelocity => _airStrafeMaxYVelocity;
        
        
        
        [SerializeField, TabGroup("Shift")] float _shiftModeSpeed;
        public float ShiftModeSpeed => _shiftModeSpeed;

        [SerializeField, TabGroup("Shift")] float _shiftModeTurnSpeed;
        public float ShiftModeTurnSpeed => _shiftModeTurnSpeed;

        [SerializeField, TabGroup("Shift")] float _sliceTeleportDistance = 15.0f;
        public float SliceTeleportDistance => _sliceTeleportDistance;

        [SerializeField, TabGroup("Shift")] float _sliceSpeedReductionAfterTeleport = 2.0f;
        public float SliceSpeedReductionAfterTeleport => _sliceSpeedReductionAfterTeleport;


        [SerializeField, TabGroup("Layers")] LayerMask _obstacleLayerMask;
        public LayerMask ObstacleLayerMask => _obstacleLayerMask;


    }
}
