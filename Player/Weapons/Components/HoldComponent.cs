using UnityEngine;

namespace Oblation.PlayerSystem.Weapons.Components
{
    public abstract class HoldComponent : Component
    {
        public abstract void OnHoldStart();
        public abstract void OnHoldContinue();
        public abstract void OnHoldRelease();

    }
}
