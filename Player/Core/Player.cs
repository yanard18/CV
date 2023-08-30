using Oblation.Blood;
using Oblation.HealthSystem;
using Oblation.Managers;
using Oblation.PlayerInputSystem;
using Oblation.PlayerSystem.Weapons;
using Oblation.SenseEngine;
using Oblation.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem
{
    /// <summary>
    /// Defines the Player class which is a SingletonWithoutCreation sub-class and represents the player in the game. 
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class Player : SingletonWithoutCreation<Player>{
        public WeaponSlot m_WeaponSlot;
        public BloodCastSlot m_BloodCastSlot;
        
        public bool m_FacingRight = true;

        /*
         * This test casts should be removed later, and bind casts different way.
         */
        [SerializeField, Required] Weapon m_TestSword;
        [SerializeField, Required] BloodCast m_TestCast;
        
        [SerializeField] SenseEnginePlayer m_SepTakeDamage;
        [SerializeField] SenseEnginePlayer m_sepDeath;

        Health m_Health;

        /// <summary>
        /// This method is called upon the object’s creation. It assigns Health, and binds the test-sword to the weapon slot.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            m_Health = GetComponent<Health>();
            m_WeaponSlot.Bind(m_TestSword);
            m_BloodCastSlot.Bind(m_TestCast);
        }

        /// <summary>
        /// This method is called every frame and evaluates the player facing direction.
        /// </summary>
        void Update()
        {
            EvaluatePlayerFacingDirection();
        }

        /// <summary>
        /// This method is called when the object becomes enabled and active, setting up health events.
        /// </summary>
        void OnEnable()
        {
            m_Health.e_OnDeath += Death;
            m_Health.e_OnDamage += OnTakeDamage;
        }

        /// <summary>
        /// This method is called when the behavior becomes disabled or inactive, cleaning up health events.
        /// </summary>
        void OnDisable()
        {
            m_Health.e_OnDeath -= Death;
            m_Health.e_OnDamage -= OnTakeDamage;
        }

        /// <summary>
        /// This method determines the direction the player is facing based on horizontal movement input.
        /// </summary>
        void EvaluatePlayerFacingDirection()
        {
            var horizontalMovementInput = PlayerInputs.m_HorizontalMovement;

            m_FacingRight = horizontalMovementInput switch
            {
                > 0 => false,
                < 0 => true,
                _ => m_FacingRight
            };
        }

        /// <summary>
        /// This method is triggered when the player takes damage, generates sense flicker, and plays the damage animation.
        /// </summary>
        void OnTakeDamage(Damage damage)
        {
            var flicker = m_SepTakeDamage.GetComponent<SenseSpriteFlicker>();
            flicker.m_Duration = m_Health.m_ImmunityDuration;
            m_SepTakeDamage.PlayIfExist();
        }

        /// <summary>
        /// This method is triggered upon the player's death, it logs who killed the player, triggers death animation, calls game over and removes the player game-object.
        /// </summary>
        void Death(Damage damage)
        {
            Debug.Log("Player killed by " + damage.m_Author.name);
            m_sepDeath.Play();
            GameManager.s_Instance.GameOver();
            Destroy(gameObject);
        }
    }

}
