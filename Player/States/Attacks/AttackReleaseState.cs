using Oblation.FSM;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace Oblation.PlayerSystem.Attacks
{
    public class AttackReleaseState : State
    {
        [SerializeField, Required] AnimationEventHandler m_AnimationEventHandler;
        [SerializeField, Required] SkeletonAnimation m_SkeletonComponent;

        public override void OnEnter()
        {
            m_AnimationEventHandler.Reset();
            var anim = m_SkeletonComponent.AnimationState.SetAnimation(0, "sword_attack/attack03_release", false);
            anim.MixDuration = 0f;
        }

        public override void OnExit()
        {
            m_AnimationEventHandler.Reset();
        }


    }
}
