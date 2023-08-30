using Oblation.FSM;

namespace Oblation.PlayerSystem.Attacks
{
    public class ChargeState : State
    {
        public override void OnEnter()
        {
            Player.s_Instance.m_WeaponSlot.m_CurrentWeapon.OnHoldEntered();
        }
        public override void Tick()
        {
            Player.s_Instance.m_WeaponSlot.m_CurrentWeapon.OnHoldContinue();
            
        }
        public override void OnExit()
        {
            Player.s_Instance.m_WeaponSlot.m_CurrentWeapon.OnHoldReleased();
        }

    }
}
