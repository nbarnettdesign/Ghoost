using UnityEngine;
using XboxCtrlrInput;

/// <summary>
/// Button state class with held timer and long-hold states
/// </summary>
public class FPS_Button : MonoBehaviour
{
    /// <summary>
    /// The name of the button input as per the Unity 'Input Editor'
    /// </summary>
    public string m_InputName;


	public KeyCode keyboardInputButton;
	public XboxButton xboxButton;
	public bool useMouse = false;
	public int mouseButton;


    /// <summary>
    /// The period that the button must be held to move into the next state
    /// </summary>
    private static float HELD_TIME = 0.5f;

    /// <summary>
    /// How long this button has been held
    /// </summary>
    private float m_fHeldTime = 0.0f;

    /// <summary>
    /// Private current button state
    /// </summary>
    private ControllerButtonState m_eButtonState = ControllerButtonState.None;

    /// <summary>
    /// Read only current state
    /// </summary>
    public ControllerButtonState CurrentState
    {
        get { return m_eButtonState; }
    }

    /// <summary>
    /// Private previous button state
    /// </summary>
    private ControllerButtonState m_ePreviousState;

    /// <summary>
    /// Read only previous state
    /// </summary>
    public ControllerButtonState PreviousState
    {
        get { return m_ePreviousState; }
    }

    /// <summary>
    /// Update this buttons state as a function of time
    /// </summary>
    /// <param name="a_fDeltaTime"></param>
    public virtual void UpdateButton(float a_fDeltaTime)
    {
        // update previous state
        m_ePreviousState = m_eButtonState;

        // Get the current down state of the button
        //bool bState = Input.GetButton(m_InputName);


		bool bState;

		if (useMouse) {
			bState = Input.GetMouseButton (mouseButton);
		} else {
			bState = Input.GetKey (keyboardInputButton) || XCI.GetButton(xboxButton);
		}



        // Update the current state based on the button press
        switch(m_eButtonState)
        {
            case ControllerButtonState.None:
            {
                if (bState)
                {
                    m_fHeldTime = a_fDeltaTime;
                    m_eButtonState = ControllerButtonState.Down;
                }
                break;
            }
            case ControllerButtonState.Down:
            {
                if (bState)
                {
                    m_fHeldTime += a_fDeltaTime;
                    m_eButtonState = ControllerButtonState.Held;
                }
                else
                {
                    m_eButtonState = ControllerButtonState.Released;
                }
                break;
            }
            case ControllerButtonState.Held:
            {
                if (bState)
                {
                    m_fHeldTime += a_fDeltaTime;
                    if (m_fHeldTime >= HELD_TIME)
                    {
                        m_eButtonState = ControllerButtonState.Long_Hold;
                    }
                }
                else
                {
                    m_eButtonState = ControllerButtonState.Released;
                }
                break;
            }
            case ControllerButtonState.Long_Hold:
            {
                if (bState)
                {
                    m_fHeldTime += a_fDeltaTime;
                }
                else
                {
                    m_eButtonState = ControllerButtonState.Released;
                }
                break;
            }
            case ControllerButtonState.Released:
            {
                if (bState)
                {
                    m_fHeldTime = a_fDeltaTime;
                    m_eButtonState = ControllerButtonState.Down;
                }
                else
                {
                    m_eButtonState = ControllerButtonState.None;
                    m_fHeldTime = 0.0f;
                }
                break;
            }
            default:
            {
                m_eButtonState = ControllerButtonState.None;
                m_fHeldTime = 0.0f;
                break;
            }
        }
    }
}
