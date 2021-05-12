using UnityEngine;

public class InputManager
{
    public static bool Right => Input.GetKey(KeyCode.RightArrow);
    public static bool Left => Input.GetKey(KeyCode.LeftArrow);
    public static bool Up => Input.GetKeyDown(KeyCode.UpArrow);
    public static bool HoldUp => Input.GetKey(KeyCode.UpArrow);
    public static bool PrimaryAttack => Input.GetKey(KeyCode.Space);
    public static bool Skill2 => Input.GetKeyDown(KeyCode.Z);
    public static bool Skill2Release => Input.GetKeyUp(KeyCode.Z);
    public static bool Skill3 => Input.GetKey(KeyCode.X);
    public static bool UltimateSkill => Input.GetKeyDown(KeyCode.C);
    public static bool UltimateRelease => Input.GetKeyUp(KeyCode.C);
    public static bool Shapeshift => Input.GetKeyDown(KeyCode.LeftShift);
    public static bool Interact => Input.GetKeyDown(KeyCode.F);
    public static bool Pause => Input.GetKeyDown(KeyCode.Escape);
    public static bool PerkMenu => Input.GetKeyDown(KeyCode.Tab);
    public static bool StatsWindow => Input.GetKeyDown(KeyCode.Q);
}
