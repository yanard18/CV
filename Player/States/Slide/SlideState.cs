using System;
using System.Collections;
using Oblation.FSM;
using Oblation.PlayerSystem.Attacks;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Oblation.PlayerSystem.Movement
{
    public class SlideState : State
    {
        public bool m_HasCooldown { get; private set; }

        [SerializeField, Required]
        SlideStateData m_Data;

        [SerializeField]
        PlayerCollision m_Collision;

        [SerializeField]
        PlayerMovementController m_MovementController;

        Rigidbody2D m_Rb;
        PlayerAttackController m_AttackController;

        float m_RelativeYVelocity;


        void Reset()
        {
            // SlideStateData
            var guids = AssetDatabase.FindAssets("t:SlideStateData");
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            m_Data = AssetDatabase.LoadAssetAtPath<SlideStateData>(path);
            
            // Collider
            var root = transform.root;
            m_Collision = root.GetComponentInChildren<PlayerCollision>();
            m_MovementController = root.GetComponent<PlayerMovementController>();
        }

        void Awake()
        {
            var root = transform.root;
            m_Rb = root.GetComponent<Rigidbody2D>();
            m_AttackController = root.GetComponent<PlayerAttackController>();
            
            m_Collision.e_OnCollisionEnter.AddListener(SaveRelativeYVelocity);
        }
        void SaveRelativeYVelocity(Collision2D col)
        {
            m_RelativeYVelocity = col.relativeVelocity.y;
        }
        public override void OnEnter()
        {
            m_AttackController.DisableAttack();
            StartCoroutine(StartCooldown(m_Data.m_CooldownDuration));
            var moveDir = Player.s_Instance.m_FacingRight ? -1 : 1;
            var effectiveX = Mathf.Abs(m_Rb.velocity.x) * m_Data.m_XPercentageToEffect / 100f;
            var effectiveY = Mathf.Abs(m_MovementController.m_SavedYVelocity) * m_Data.m_YPercentageToEffect / 100f;
            Debug.Log(m_MovementController.m_SavedYVelocity);
            var speed = effectiveX + effectiveY + m_Data.m_ExtraSpeed;
            speed = Mathf.Min(speed, m_Data.m_MaxSpeed);
            speed *= moveDir;
            m_Rb.velocity = m_Rb.velocity.With(x: speed);
        }
        public override void PhysicsTick()
        {
            SlowDown();
        }
        public override void OnExit()
        {
            m_AttackController.EnableAttack();
        }
        void SlowDown()
        {
            var currentXVelocity = m_Rb.velocity.x;
            currentXVelocity = Mathf.MoveTowards(currentXVelocity, 0, Time.fixedDeltaTime * m_Data.m_FrictionAcceleration);
            m_Rb.velocity = new Vector2(currentXVelocity, m_Rb.velocity.y);
        }

        IEnumerator StartCooldown(float duration)
        {
            if (m_HasCooldown) yield break;

            m_HasCooldown = true;
            yield return new WaitForSeconds(duration);
            m_HasCooldown = false;
        }
    }
}
