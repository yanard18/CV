using Oblation.Blood;
using UnityEngine;

namespace Oblation.Projectiles
{
    public class BloodProjectile : Projectile
    {

        [SerializeField]
        BloodCapacitor m_Capacitor;
        
        BloodCollector m_BloodCollector;

        void Start()
        {
            m_BloodCollector.CollectBlood(1f, m_Capacitor);
        }

        protected override void Awake()
        {
            base.Awake();
            m_BloodCollector = GetComponent<BloodCollector>();
        }

        public void KillBloodStream()
        {
            m_Capacitor.KillAllBloodStreams();
        }



    }
}
