using UnityEngine;

namespace Oblation.PlayerSystem.Weapons
{
    public class WeaponSlot : MonoBehaviour
    {
        public Weapon m_CurrentWeapon;
        
        public void Bind(Weapon weapon)
        {
            var transform1 = transform;
            var spawnedWeapon = Instantiate(weapon, transform1.position, Quaternion.identity, transform1);
            m_CurrentWeapon = spawnedWeapon;
        }

        public void Unbind()
        {
            Destroy(m_CurrentWeapon);
            m_CurrentWeapon = null;
        }
        
    }
}
