using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FPS_HeightTrigger : MonoBehaviour
{
    /// <summary>
    /// Player reference for mask and push variables
    /// </summary>
    public FPS_Player m_Player;

    /// <summary>
    /// List for responsive ridgid bodies
    /// </summary>
    private List<Rigidbody> m_RigidBodyList;

    /// <summary>
    /// Temporary rigid body for current collidable
    /// </summary>
    private Rigidbody t_RigidBody;

    /// <summary>
    /// Counter for number of colliders currently in this trigger
    /// </summary>
    public int m_iCount = 0;

    /// <summary>
    /// Returns true if colliding with anything
    /// </summary>
    public bool Colliding
    {
        get { return m_iCount > 0 ? true : false; }
    }

    /// <summary>
    /// Number of colliders in the trigger
    /// </summary>
    public int ColliderCount
    {
        get { return m_iCount;  }
    }

    /// <summary>
    /// Returns the first Rigidbody in the list if present
    /// </summary>
    public Rigidbody FirstRigidBody
    {
        get 
        { 
            return ( m_RigidBodyList.Count == 0? null:m_RigidBodyList[0]);
        }
    }

    /// <summary>
    /// Returns true if ONLY colliding with pushables / placeables
    /// </summary>
    public bool CollidingPlaceables
    {
        get { return m_iCount == m_RigidBodyList.Count && Colliding ? true : false; }
    }

    /// <summary>
    /// Get the layers in question
    /// </summary>
    private void Awake()
    {
        m_RigidBodyList = new List<Rigidbody>();
    }

    /// <summary>
    /// Apply force to placeables overlapping this trigger as per player strength
    /// </summary>
    public void ForceOut(ForceMode a_ForceMode = ForceMode.Force)
    {
        for (int i = 0; i < m_RigidBodyList.Count; ++i)
        {
            m_RigidBodyList[i].AddForceAtPosition
                ((m_RigidBodyList[i].position - transform.position).normalized * (a_ForceMode == ForceMode.Impulse ? m_Player.HitStrength : m_Player.PushStrength),
                transform.position,
                a_ForceMode);
        }
    }

    /// <summary>
    /// Track any colliders that intersect this trigger
    /// </summary>
    /// <param name="other">Collider that entered the trigger</param>
	public void OnTriggerEnter(Collider other)
    {

		if (!other.name.Contains ("Capsule_Check") && !other.name.Contains ("FPS_Player") ) {
			//Debug.Log (other.name);

	        // Increment the count
	        ++m_iCount;

	        // Get the contacted rigidbody, if any
	        t_RigidBody = null;
	        t_RigidBody = other.transform.GetComponent<Rigidbody>();
	        
	        // Apply force based on layer and reletive displacement
	        if (t_RigidBody != null)
	        {
	            if (!t_RigidBody.isKinematic)
	            {
	                m_RigidBodyList.Add(t_RigidBody);
	            }
	        }
		}
    }

    /// <summary>
    /// Deregister the collider
    /// </summary>
    /// <param name="other">Collider that left the trigger</param>
    public void OnTriggerExit(Collider other)
    {
		if (!other.name.Contains ("Capsule_Check") && !other.name.Contains ("FPS_Player")) {

			// Decrement counter
			--m_iCount;
			// Get the contacted rigidbody
			t_RigidBody = null;
			t_RigidBody = other.transform.GetComponent<Rigidbody> ();

			// Apply force based on layer and reletive displacement
			if (t_RigidBody != null) {
				if (!t_RigidBody.isKinematic) {
					m_RigidBodyList.Remove (t_RigidBody);
				}
			}
		}
    }
}
