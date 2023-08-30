using UnityEngine;

namespace Oblation.PlayerSystem.Weapons.Components
{
    public class HoldComponentCharge : HoldComponent
    {
        [SerializeField] string m_ChargeAnimationName;

        [SerializeField] string m_ReleaseAnimationName;

        public override void OnHoldStart()
        {
            var anim = m_SkeletonComponent.AnimationState.SetAnimation(0, m_ChargeAnimationName, true);
            anim.MixDuration = 0f;
        }
        public override void OnHoldContinue()
        {
        }
        public override void OnHoldRelease()
        {
            var anim = m_SkeletonComponent.AnimationState.SetAnimation(0, m_ReleaseAnimationName, false);
            anim.MixDuration = 0f;
        }
    }
}
