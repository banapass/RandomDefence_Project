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
    None, UnitPlacement
}

[System.Serializable]
public enum UnitRarity
{
    Common, Uncommon, Rare, Epic, Legendary
}
