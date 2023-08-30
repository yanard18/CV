using System.Collections;
using Oblation.FSM;
using UnityEngine;

///<summary>
/// UseOneWayPlatformState
///</summary>
public class PassPlatformState : State
{
    public bool m_HasCooldown { get; private set; }
    public GameObject m_CurrentPlatform { get; private set; }

    [SerializeField]
    Collider2D m_PlayerCollider;



    public override void OnEnter()
    {
        if (m_CurrentPlatform == null)
            return;


        DisableCollision();
        StartCoroutine(CreateCooldown(.2f));

    }

    void Reset()
    {
        m_PlayerCollider = transform.root.Find("Collider").GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag($"OneWayPlatform"))
            m_CurrentPlatform = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag($"OneWayPlatform")) return;
        EnableCollision();
        m_CurrentPlatform = null;
    }

    void DisableCollision()
    {
        var col = m_CurrentPlatform.GetComponent<CompositeCollider2D>();
        Physics2D.IgnoreCollision(col, m_PlayerCollider);
    }

    void EnableCollision()
    {
        var col = m_CurrentPlatform.GetComponent<CompositeCollider2D>();
        Physics2D.IgnoreCollision(col, m_PlayerCollider, false);
    }

    IEnumerator CreateCooldown(float duration)
    {
        m_HasCooldown = true;
        yield return new WaitForSeconds(duration);
        m_HasCooldown = false;
    }

}
