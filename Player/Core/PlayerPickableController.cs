using UnityEngine;

namespace Oblation
{
    /// <summary>
    /// PlayerPickableController checks if there are pickable object nearby and if yes handle the pickup.
    /// </summary>
    public class PlayerPickableController : MonoBehaviour
    {
        [SerializeField] float m_Range = 3f;
        void Update()
        {
            var contacts = Physics2D.OverlapCircleAll(transform.position, m_Range);
            foreach (var contact in contacts)
            {
                var pickable = contact.FindComponent<Pickable>();
                if(pickable == null) continue;
                pickable.PickUp(transform.root.gameObject);
                return;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_Range);
        }
    }
    
}
