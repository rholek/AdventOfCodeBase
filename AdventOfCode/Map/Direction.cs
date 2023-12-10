namespace AdventOfCode.Map;

[Flags]
public enum Direction
{
    Up = 0b0001,
    UpRight = Up | Right,
    Right = 0b0010,
    DownRight = Down | Right,
    Down = 0b0100,
    DownLeft = Down | Left,
    Left = 0b1000,
    UpLeft = Up | Left
}
