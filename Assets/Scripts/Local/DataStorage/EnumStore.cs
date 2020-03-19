//Stores all public enums used by multiple classes

#region Color 
public enum ColorPalettes
{
    ColorPalette_01,
    ColorPalette_02
}

public enum Colors //Used by PlayerController and Obstacles
{
    //Yellow,
    //Magenta,
    //Cyan,
    //Vermillion
    Color0,
    Color1,
    Color2,
    Color3
}
#endregion

#region Obstacles
public enum ObstacleEffect
{
    NullifyVelocity,
    AddForce,
    ScoreMultiplier
}

public enum ObstaclePattern
{
    Circle,
    Square,
    Wave
}

public enum CourseDifficulty
{
    Beginner,
    Intermediate,
    Expert,
    Hellish
}
#endregion
