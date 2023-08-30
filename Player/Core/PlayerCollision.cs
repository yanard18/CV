using UnityEngine;
using UnityEngine.Events;

namespace Oblation.PlayerSystem
{
    public class PlayerCollision : MonoBehaviour
    {
        public UnityEvent<Collision2D> e_OnCollisionEnter;
        public UnityEvent<Collision2D> e_OnCollisionStay;
        public UnityEvent<Collision2D> e_OnCollisionExit;

        void OnCollisionEnter2D(Collision2D other)
        {
            e_OnCollisionEnter?.Invoke(other);
        }
        void OnCollisionStay2D(Collision2D other)
        {
            e_OnCollisionStay?.Invoke(other);
        }
        void OnCollisionExit2D(Collision2D other)
        {
            e_OnCollisionExit?.Invoke(other);
        }


    }
}
