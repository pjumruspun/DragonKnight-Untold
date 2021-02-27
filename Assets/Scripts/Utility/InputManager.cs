using UnityEngine;

public class InputManager
{
    public static bool Right => Input.GetKey(KeyCode.RightArrow);
    public static bool Left => Input.GetKey(KeyCode.LeftArrow);
    public static bool Up => Input.GetKeyDown(KeyCode.UpArrow);
    public static bool HoldUp => Input.GetKey(KeyCode.UpArrow);
    public static bool PrimaryAttack => Input.GetKey(KeyCode.Space);
    public static bool Skill2 => Input.GetKeyDown(KeyCode.Z);
    public static bool Shapeshift => Input.GetKeyDown(KeyCode.LeftShift);
}
