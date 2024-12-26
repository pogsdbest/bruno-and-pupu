using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    public Texture2D Texture;
    public List<Sprite> SpriteList = new List<Sprite>();

    void Awake()
    {
        string texturePath = AssetDatabase.GetAssetPath(Texture);
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath);

        foreach (var sprite in sprites)
        {
            if(sprite is Sprite)
            {
                if (sprite.name.StartsWith('-')) continue;

                SpriteList.Add((sprite as Sprite));
            }
                
        }
    }

    public Sprite GetSprite(int id)
    {
        return SpriteList[id];
    }
}
