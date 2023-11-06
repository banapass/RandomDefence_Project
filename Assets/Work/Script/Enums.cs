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
    None, UnitPlacement, Unit, Sell
}

[System.Serializable]
public enum UnitRarity
{
    None,
    Common,
    Uncommon,
    Epic,
    Legendary
}

public enum DebuffType
{
    None = 0, Slow, Bleed
}

public enum CostType
{
    Wall, Unit
}
public enum PlacementTileType
{
    None, Unit
}

[System.Serializable]
public enum MonsterType
{
    Normal, Boss
}

public enum SoundType
{
    Music, SFX
}

public enum SpaceType
{
    World, Local
}

public enum GameSpeed
{
    Pause,
    Normal,
    Double,
    Max

}