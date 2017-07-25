using UnityEngine;
using System.Collections;

public class FPS_GroundForce : MonoBehaviour
{
    /// <summary>
    /// Reference to the Player component to get the motion and mass for momentum calculations
    /// </summary>
    public FPS_Player m_Player;

    /// <summary>
    /// Temporary rigid body for current collidable
    /// </summary>
    private Rigidbody t_RigidBody;

    /// <summary>
    /// Temporary vector got the math
    /// </summary>
    private Vector3 t_Vector3;

    /// <summary>
    /// How to react on controller collision with rigid body object
    /// </summary>
    /// <param name="a_collision">Collider that the grounded trigger is contacting</param>
    private void OnTriggerStay(Collider a_collider)
    {
        // Get the contacted rigidbody
        t_RigidBody = null;
        t_RigidBody = a_collider.transform.GetComponent<Rigidbody>();
        
        // Apply force based on layer and reletive displacement
        if (t_RigidBody != null)
        {
            if (!t_RigidBody.isKinematic)
            {
                t_RigidBody.AddForceAtPosition(
                        Vector3.up * m_Player.Mass * m_Player.Gravity,
                        m_Player.transform.position,
                        ForceMode.Force);
            }
        }
    }
}
