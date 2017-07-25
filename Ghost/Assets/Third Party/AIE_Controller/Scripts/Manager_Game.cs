using UnityEngine;

/// <summary>
/// Game Manager works to poll input and update the player
/// </summary>
public class Manager_Game : MonoBehaviour
{
    /// <summary>
    /// Input manager reference
    /// </summary>
    public FPS_InputManager m_InputManager;
    /// <summary>
    /// Player script reference
    /// </summary>
    public FPS_Player m_Player;

    /// <summary>
    /// Unity Delegate Initialisation Point
    /// </summary>
    void Awake()
    {
        m_InputManager.Initialise();
    }

    /// <summary>
    /// Unity Delegate per frame Update
    /// </summary>
    void Update()
    {
        m_InputManager.Iterate(Time.deltaTime);
        m_Player.Iterate(Time.deltaTime);
    }


}
