using UnityEngine;

public class InputManager
{
    public static bool Right => Input.GetKey(KeyCode.D);
    public static bool Left => Input.GetKey(KeyCode.A);
    public static bool Up => Input.GetKeyDown(KeyCode.W);
    public static bool HoldUp => Input.GetKey(KeyCode.W);
    public static bool PrimaryAttack => Input.GetKeyDown(KeyCode.Space);
    public static bool Shapeshift => Input.GetKeyDown(KeyCode.LeftShift);
}
