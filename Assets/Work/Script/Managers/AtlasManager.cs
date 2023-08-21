using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using framework;
public class AtlasManager : Singleton<AtlasManager>
{
    [SerializeField] SpriteAtlas atlas;

    private const string UNIT_RARITY_FORMAT = "unitslot-{0}";

    public void Init()
    {
        ResourceStorage.GetObjectRes<SpriteAtlas>("Atlas", _atlas => atlas = _atlas);
    }
    public Sprite GetSprite(string _spriteName)
    {
        return atlas.GetSprite(_spriteName);
    }

    public Sprite GetUnitRarityTileSprite(UnitRarity _rarity)
    {
        return GetSprite(string.Format(UNIT_RARITY_FORMAT, _rarity));
    }
}
