using UnityEngine;

public class Constants
{
    public static readonly Vector2Int MAP_SIZE = new Vector2Int(14, 9);
    public const float PROJECTILE_LIFETIME = 5.0f;
    public const float MAX_SLOW = 0.8f;
    public const string DEBUFF_KEY = "Debuff";
    public const int MAX_LIFE = 5;
    public const int START_GOLD = 500;
    public const int UNITPLACEMENT_PRICE = 10;
    public const int UNIT_PRICE = 50;
    public const int UNIT_UPGRADE_COUNT = 3;


    public const string UNITPLACEMENT_PATH = "Prefab/Unitplacement";
    public const string FLOATING_TEXT = "Prefab/Effect/FloatingText";
    public const string MONSTER_DEAD_KEY = "DeadEffect";
    public const string MONSTER_HPBAR = "Monster_Hpbar";

    public const float REFERANCE_WIDTH = 1920;
    public const float REFERANCE_HEIGHT = 1080;

    public static readonly Vector2 REFERANCE_SIZE = new Vector2(1920, 1080);

}