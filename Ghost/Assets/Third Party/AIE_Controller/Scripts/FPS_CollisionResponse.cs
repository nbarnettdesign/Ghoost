using UnityEngine;
using System.Collections;

/// <summary>
/// Due to the non-ridgidbody nature of the capsule collider,
/// Push and kick objects on hit where 
/// </summary>
public class FPS_CollisionResponse : MonoBehaviour
{
    /// <summary>
    /// Reference to the Player component to get the motion and mass for momentum calculations
    /// </summary>
    public FPS_Player m_Player;

    /// <summary>
    /// Character controller reference for physics access
    /// </summary>
    public CharacterController m_CharacterController;
    
    /// <summary>
    /// Temporary rigid body for current collidable
    /// </summary>
    private Rigidbody t_RigidBody;

    /// <summary>
    /// Temporary vector got the math
    /// </summary>
    private Vector3 t_Vector3;

    /// <summary>
    /// Temporary float storage of the dot product
    /// </summary>
    private float t_fDot; 

    /// <summary>
    /// How to react on controller collision with rigid body object
    /// </summary>
    /// <param name="a_collision"></param>
    private void OnControllerColliderHit(ControllerColliderHit a_collision)
    {
        // Get the contacted rigidbody
        t_RigidBody = null;
        t_RigidBody = a_collision.transform.GetComponent<Rigidbody>();
        
        // Apply force based on layer and reletive displacement
        if (t_RigidBody != null)
        {
            // Displacement calculation to collision point
            t_Vector3 = a_collision.point - transform.position;
            t_Vector3.y = 0;
            t_Vector3.Normalize();
            t_fDot = Vector3.Dot(t_Vector3, a_collision.moveDirection);
            
            // Apply force to object if valid, as a function of motion, and strength
            if (!t_RigidBody.isKinematic)
            {
                if (t_fDot > 0)
                {
                    t_RigidBody.AddForceAtPosition(
                            t_Vector3 * t_fDot * m_Player.PushStrength * m_Player.Motion.magnitude,
                            a_collision.point,
                            ForceMode.Force);
                }
            }
        }
    }
}
