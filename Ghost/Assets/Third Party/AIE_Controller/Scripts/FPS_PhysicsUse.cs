using System;
using UnityEngine;

/// <summary>
/// Controlls camera raycast projections to 
///     manipulate rigidbodies from an FPS perspective
/// </summary>
public class FPS_PhysicsUse : MonoBehaviour
{
    /// <summary>
    /// The main POV Camera
    /// </summary>
    public Camera m_RayCastCamera;
    
    /// <summary>
    /// Emission Colour of selected object
    /// </summary>
    public Color m_cSelected;

    /// <summary>
    /// Grounded trigger reference to make sure we are not standing
    /// on the object we are trying to grab.
    /// </summary>
    public FPS_HeightTrigger m_HeightGrounded;

    /// <summary>
    /// Move held object on X axis
    /// </summary>
    public FPS_Axis m_AxisMoveX;

    /// <summary>
    /// Move held object on Y axis
    /// </summary>
    public FPS_Axis m_AxisMoveY;

    /// <summary>
    /// Move held object on Z axis
    /// </summary>
    public FPS_Axis m_AxisMoveZ;

    /// <summary>
    /// Rotate held object on X axis
    /// </summary>
    public FPS_Axis m_AxisRotateX;

    /// <summary>
    /// Rotate held object on Y axis
    /// </summary>
    public FPS_Axis m_AxisRotateY;

    /// <summary>
    /// Rotate held object on Z axis
    /// </summary>
    public FPS_Axis m_AxisRotateZ;

    /// <summary>
    /// Current GameObject being manipulated
    /// </summary>
    private GameObject m_PhysicsObject;

    /// <summary>
    /// Read only access to held object
    /// </summary>
    public GameObject HeldObject
    {
        get { return m_PhysicsObject; }
    }

    /// <summary>
    /// Current GameObject being manipulated
    /// </summary>
    private Rigidbody m_Rigidbody;

    /// <summary>
    /// All renderer on the selected object
    /// </summary>
    private Renderer m_Renderer;

    /// <summary>
    /// All renderers on the selected object
    /// </summary>
    private Renderer[] m_Renderers;

    /// <summary>
    /// Drag of the object before applying manipulation dampening
    /// </summary>
    private float m_fDrag;

    /// <summary>
    /// Emission Colour of selected object before selection
    /// </summary>
    private Color m_cEmission;

    /// <summary>
    /// Emission Colour of selected object before selection
    /// </summary>
    private Color[] m_cEmissions;

    /// <summary>
    /// The displacement offset of the object
    /// </summary>
    private Vector3 m_v3Offset;

    /// <summary>
    /// The rotation offset of the object
    /// </summary>
    private Quaternion m_qRotation;

    /// <summary>
    /// Range of the raycast from the camera
    /// </summary>
    private float m_fRange = 3.0f;

    /// <summary>
    /// Range of the raycast from the camera
    /// </summary>
    private float m_fMaxRange = 5.0f;

    /// <summary>
    /// Drag during manipulation
    /// </summary>
    private float m_fManipulationDrag = 5.0f;

    /// <summary>
    /// Local ray storage
    /// </summary>
    Ray m_Ray;

    /// <summary>
    /// Local hit result storage
    /// </summary>
    RaycastHit m_RaycastHit;

    /// <summary>
    /// Collision mask to ignore the player's triggers
    /// </summary>
    int iMask = 1;

    /// <summary>
    /// On instantiation, ignore the layer that is this player
    /// </summary>
    public void Awake()
    {
        iMask = 1 << gameObject.layer;
        iMask = ~iMask;
    }

    /// <summary>
    /// Atempt to find and engage a rigidbody
    /// </summary>
    public bool Engage(float a_fWeight)
    {
        // Cast ray from centre of screen
        m_Ray = m_RayCastCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        if (Physics.Raycast(m_Ray, out m_RaycastHit, m_fRange, iMask))
        {
            m_Rigidbody = m_RaycastHit.collider.GetComponent<Rigidbody>();
            // If we hit something with a rigid body
            if (m_Rigidbody != null)
            {
                // Can we pick it up?
                if (a_fWeight < m_Rigidbody.mass || m_Rigidbody.isKinematic)
                {
                    return false;
                }

                // Then this is our engaged game object
                m_PhysicsObject = m_RaycastHit.collider.gameObject;
                // Store a positional offset accounting for the players rotation
                m_v3Offset = Quaternion.Inverse(m_RayCastCamera.transform.rotation) * (m_Rigidbody.position - m_RayCastCamera.transform.position);
                // Toggle off Gravity
                m_Rigidbody.useGravity = false;
                // Store...
                m_fDrag = m_Rigidbody.drag;
                // and assign drag.
                m_Rigidbody.drag = m_fManipulationDrag;
                // Continuous Dynamic detection
                m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                // Get the renderers and assign an emission colour
                m_Renderer = m_PhysicsObject.GetComponent<Renderer>();
                if (m_Renderer != null)
                {
                    m_cEmission = m_Renderer.material.GetColor("_EmissionColor");
                    m_Renderer.material.SetColor("_EmissionColor", m_cSelected);
                }
                else
                {
                    m_Renderers = m_PhysicsObject.GetComponentsInChildren<Renderer>();
                    if (m_Renderers != null)
                    {
                        m_cEmissions = new Color[m_Renderers.Length];
                        for (int i = 0; i < m_Renderers.Length; ++i)
                        {
                            m_cEmissions[i] = m_Renderers[i].material.GetColor("_EmissionColor");
                            m_Renderers[i].material.SetColor("_EmissionColor", m_cSelected);
                        }
                    }
                }
                return true;
            }
        }

        // We didn't find a target, or it was invalid
        m_PhysicsObject = null;
        return false;
    }
    
    /// <summary>
    /// Per frame updating of position and interaction
    /// </summary>
    public bool Iterate()
    {
        // If theres no object - Done
        if (m_PhysicsObject == null)
        {
            return false;
        }

        // Calculate displacement vector
        Vector3 displacement = (m_RayCastCamera.transform.position +
                                m_RayCastCamera.transform.rotation * m_v3Offset) -
                                m_Rigidbody.position;

        // Add force towards the offset point
        float fSqrMag = displacement.sqrMagnitude;
        m_Rigidbody.AddForce(displacement.normalized * fSqrMag, ForceMode.VelocityChange);

        // Drop if out of range
        if ((m_Rigidbody.position - transform.position).magnitude > m_fMaxRange)
        {
            return false;
        }

        // If being stood on, drop
        if (m_HeightGrounded.CollidingPlaceables 
            && m_HeightGrounded.ColliderCount == 1
            && m_HeightGrounded.FirstRigidBody == m_Rigidbody)
        {
            return false;
        }

        // Othewise, continue carrying
        return true;
    }
    
    /// <summary>
    /// Rotate the object based on mouse motion
    /// </summary>
    public bool Manipulate()                           
    {
        if (m_Rigidbody == null)
        {
            return false;
        }
        
        // Apply torque to object based on mouse motion
        Vector3 t_Vector3 = new Vector3(
            m_AxisRotateX.AxisValue * Time.deltaTime,
            -m_AxisRotateY.AxisValue * Time.deltaTime,
            m_AxisRotateZ.AxisValue * Time.deltaTime);
        t_Vector3 = transform.rotation * t_Vector3;
        m_Rigidbody.AddTorque(t_Vector3, ForceMode.VelocityChange);

        // Add to offset based on WASD axes
        m_v3Offset += (m_AxisMoveX.AxisValue * Vector3.right * Time.deltaTime);
        m_v3Offset += (m_AxisMoveY.AxisValue * Vector3.up * Time.deltaTime);
        m_v3Offset += (m_AxisMoveZ.AxisValue * Vector3.forward);
        return true;
    }

    /// <summary>
    /// Reset the render and physics properties once we are done
    ///   manipulating the object
    /// </summary>
    public void Disengage()
    {
        if (m_PhysicsObject != null)
        {
            m_Rigidbody.useGravity = true;
            m_Rigidbody.drag = m_fDrag;

            // Continuous Dynamic detection
            m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;

            if (m_Renderer != null)
            {
                m_Renderer.material.SetColor("_EmissionColor", m_cEmission);
            }
            else if (m_Renderers != null)
            {
                for (int i = 0; i < m_Renderers.Length; ++i)
                {
                    m_Renderers[i].material.SetColor("_EmissionColor", m_cEmissions[i]);
                }
            }
            m_PhysicsObject = null;
        }
    }
}
