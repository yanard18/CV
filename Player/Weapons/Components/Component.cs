using Spine.Unity;
using UnityEngine;

namespace Oblation.PlayerSystem.Weapons.Components
{
    public class Component : MonoBehaviour
    {
        protected SkeletonAnimation m_SkeletonComponent;
        protected virtual void Awake()
        {
            m_SkeletonComponent = transform.root.FindComponent<SkeletonAnimation>();
        }
    }
}
