using UnityEngine;
using XboxCtrlrInput;

/// <summary>
/// Wrapper for UnityInput axis with more functionallity
/// </summary>
public class FPS_Axis : MonoBehaviour
{
    /// <summary>
    /// The name of the axis as per the Unity 'Input Editor'
    /// </summary>
    public string m_AxisName;

	public XboxAxis xboxAxis;
    /// <summary>
    /// Boolean flag as to inver the values of this axis
    /// </summary>
    public bool m_bInvertAxis;
    /// <summary>
    /// Most recent value of this axis 
    /// </summary>
    private float m_fValue;

    /// <summary>
    /// Most recent axis value - Read only
    /// </summary>
    public float AxisValue
    {
        get { return m_bInvertAxis ? -m_fValue : m_fValue ; }
    }

    /// <summary>
    /// Convert the axis into absolute up/down/none ( 1 / -1 / 0 )
    /// </summary>
    public int AxisSign
    {
        get
        {
            if (AxisValue == 0.0f)
            {
                return 0;
            }
            return (AxisValue > 0 ? 1 : -1);
        }
    }
    
    /// <summary>
    /// Update the Axis value as per Input Editor
    /// </summary>
    public void UpdateAxis()
    {
		if (Input.GetAxis (m_AxisName) == 0) {
			m_fValue = XCI.GetAxis(xboxAxis);
		} else {
			m_fValue = Input.GetAxis(m_AxisName);
		}
        
    }
}
