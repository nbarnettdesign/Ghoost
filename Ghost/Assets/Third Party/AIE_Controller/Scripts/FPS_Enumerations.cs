/// <summary>
/// State indicating height of the controller and view
/// </summary>
public enum Pose
{
    Stand,
    Crouch,
    Prone
}

/// <summary>
/// State indicating whether character is jumping
/// </summary>
public enum Jump
{
    Grounded,
    Jumping,
    InAir
}

/// <summary>
/// Button States for input
/// </summary>
public enum ControllerButtonState
{
    None,
    Down,
    Held,
    Long_Hold,
    Released
}