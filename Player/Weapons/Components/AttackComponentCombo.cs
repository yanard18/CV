using Oblation.SenseEngine;
using Spine.Unity;
using UnityEngine;

namespace Oblation.PlayerSystem.Weapons.Components
{
    public class AttackComponentCombo : AttackComponent
    {

        [SerializeField] string[] m_AnimationList;
        [SerializeField] SenseEnginePlayer m_ComboSEP;

        Rigidbody2D m_Rb;

        int m_Index;

        protected override void Awake()
        {
            base.Awake();
            m_Rb = Player.s_Instance.GetComponent<Rigidbody2D>();
        }

        public override void Use()
        {
            m_SkeletonComponent = Player.s_Instance.GetComponentInChildren<SkeletonAnimation>();
            var anim = m_SkeletonComponent.AnimationState.SetAnimation(0, m_AnimationList[m_Index], false);
            anim.MixDuration = 0f;
            m_SkeletonComponent.AnimationState.AddEmptyAnimation(0, 0, 0);

            m_Rb.velocity = m_Rb.velocity.With(x: 0);
            
            m_Index++;
            m_ComboSEP.PlayIfExist();

            if (m_Index > m_AnimationList.Length - 1)
                m_Index = 0;

        }
    }
}
