using Oblation.PlayerSystem.Weapons.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField, Required] AttackComponent m_AttackComponent;
        [SerializeField, Required] AttackComponent m_AirAttackComponent;

        [SerializeField, Required] HoldComponent m_HoldComponent;

        public void Attack()
        {
            m_AttackComponent.Use();
        }
        public void AirAttack()
        {
            m_AirAttackComponent.Use();
        }

        public void OnHoldEntered()
        {
            m_HoldComponent.OnHoldStart();
        }
        public void OnHoldContinue()
        {
            m_HoldComponent.OnHoldContinue();
            
        }
        public void OnHoldReleased()
        {
            m_HoldComponent.OnHoldRelease();
        }

    }
}
