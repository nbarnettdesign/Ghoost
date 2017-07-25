using UnityEngine;

/// <summary>
/// Class updates all child buttons and axes
/// </summary>
public class FPS_InputManager : MonoBehaviour
{
    /// <summary>
    /// Parent gameobject of input button objects
    /// </summary>
    public GameObject m_ButtonObject;

    /// <summary>
    /// List of all located child button scripts
    /// </summary>
    private FPS_Button[] m_Buttons;
    
    /// <summary>
    /// Parent gameobject of input button objects
    /// </summary>
    public GameObject m_AxisObject;

    /// <summary>
    /// List of all located child axis scripts
    /// </summary>
    private FPS_Axis[] m_Axes;

    /// <summary>
    /// Gathers all buttons and Axes
    /// </summary>
    public void Initialise()
    {
        m_Buttons = m_ButtonObject.GetComponentsInChildren<FPS_Button>();
        m_Axes = m_AxisObject.GetComponentsInChildren<FPS_Axis>();
    }

    /// <summary>
    /// Updates button and axis states
    /// </summary>
    /// <param name="a_fDeltaTime"></param>
    public void Iterate(float a_fDeltaTime)
    { 
	    for(int i = 0; i < m_Buttons.Length; ++i)
        {
            m_Buttons[i].UpdateButton(a_fDeltaTime);
        }

        for (int i = 0; i < m_Axes.Length; ++i)
        {
            m_Axes[i].UpdateAxis();
        }
    }
}
