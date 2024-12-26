using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    public Texture2D Texture;
    public List<Sprite> SpriteList = new List<Sprite>();

    void Awake()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("CardsTexture");

        foreach (var sprite in sprites)
        {
            if (sprite.name.StartsWith("-")) continue;
            SpriteList.Add(sprite);
        }
    }

    public Sprite GetSprite(int id)
    {
        return SpriteList[id];
    }
}
