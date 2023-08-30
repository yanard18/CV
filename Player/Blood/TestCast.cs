using Oblation.PlayerInputSystem;
using Oblation.PlayerSystem;
using Oblation.Projectiles;
using UnityEngine;

namespace Oblation.Blood
{

    [CreateAssetMenu(fileName = "Blood Cast", menuName = "Oblation/Blood/Casts/Test Cast", order = 0)]
    public class TestCast : BloodCast
    {
        [SerializeField]
        BloodProjectile m_BloodProjectile;

        BloodProjectile m_Projectile;
        BloodCapacitor m_Capacitor;

        Vector2 m_Velocity;

        public override void Cast()
        {
            var mousePos = PlayerInputs.GetMouseWorldPos();
            Vector2 playerPos = Player.s_Instance.transform.position + Vector3.up;
            var dir = playerPos.Direction(mousePos, true);
            var spawnPos = playerPos + dir;
            var angle = Vector2.SignedAngle(Vector2.right, dir);

            m_Projectile = Instantiate(m_BloodProjectile, spawnPos, Quaternion.AngleAxis(angle, Vector3.forward));
            m_Capacitor = m_Projectile.GetComponent<BloodCapacitor>();
            m_Projectile.transform.localScale = Vector3.one * .1f;
        }

        public override void Tick()
        {
            // Set size
            m_Projectile.transform.localScale = Vector3.Lerp(Vector3.one * .1f, Vector3.one, m_Capacitor.GetBloodAmount() / m_Capacitor.GetCapacity());

            // Set position
            var mousePos = PlayerInputs.GetMouseWorldPos();
            Vector2 playerPos = Player.s_Instance.transform.position + Vector3.up;
            var dir = playerPos.Direction(mousePos, true) * 1.25f;
            var targetPos = playerPos + dir;
            m_Projectile.transform.position = Vector2.SmoothDamp(m_Projectile.transform.position, targetPos, ref m_Velocity, .1f, 20f);

        }

        public override void Release()
        {
            var capacitor = m_Projectile.GetComponent<BloodCapacitor>();
            if (capacitor.GetBloodAmount() < 5)
            {

                m_Projectile.KillBloodStream();
                InvokeOnECastCompleted();
                Destroy(m_Projectile.gameObject);
            }
            else
            {
                var mousePos = PlayerInputs.GetMouseWorldPos();
                Vector2 pos = m_Projectile.transform.position;
                var dir = pos.Direction(mousePos, true);
                m_Projectile.Init(dir * 10f, author: Player.s_Instance.gameObject);
                m_Projectile.KillBloodStream();
                InvokeOnECastCompleted();
            }
        }

    }
}
