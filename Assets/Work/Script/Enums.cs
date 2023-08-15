public enum Orientation
{
    Landscape,
    Portrait
    // Add more orientations if needed
}

public enum GameState
{
    None, Playing, BreakTime, GameOver, GameClear
}

public enum PlacementState
{
    None, UnitPlacement, Unit
}

[System.Serializable]
public enum UnitRarity
{
    None,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum DebuffType
{
    None = 0, Slow
}