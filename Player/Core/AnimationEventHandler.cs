using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Oblation.PlayerSystem
{
    public class AnimationEventHandler : MonoBehaviour
    {
        [HideInInspector]
        public bool m_CanBreak;
        [HideInInspector]
        public bool m_CanCharge;
        [HideInInspector]
        public bool m_HasFinished;

        [SerializeField, Required] SkeletonAnimation m_SkeletonComponent;

        public void Reset()
        {
            m_CanBreak = false;
            m_CanCharge = false;
            m_HasFinished = false;
        }
        void Awake()
        {
            m_SkeletonComponent.AnimationState.Event += OnAnimationEvent;
            m_SkeletonComponent.AnimationState.Complete += delegate
            {
                m_HasFinished = true;
            };
        }


        void OnAnimationEvent(TrackEntry trackentry, Event e)
        {
            switch (e.Data.Name)
            {
                case "Attack Can Break":
                    m_CanBreak = true;
                    break;
                case "Attack Can Charge":
                    m_CanCharge = true;
                    break;
                case "Attack Deal Damage":
                    break;
            }
        }


    }
}
