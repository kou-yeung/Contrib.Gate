using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class SpritePostprocess : AssetPostprocessor
{
    public override int GetPostprocessOrder()
    {
        return int.MaxValue;
    }

    void OnPostprocessTexture(Texture2D texture)
    {
        switch (Path.GetFileNameWithoutExtension(assetPath))
        {
            case "face":
                Split(texture, new Vector2Int(4, 2), SpriteAlignment.Center);
                break;
            case "walk":
                Split(texture, new Vector2Int(3, 4), SpriteAlignment.BottomCenter);
                break;
            case "base":
                {
                    var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                    importer.textureType = TextureImporterType.Sprite;
                    AssetDatabase.SaveAssets();
                }
                break;
        }
    }

    void Split(Texture2D texture, Vector2Int grid, SpriteAlignment alignment)
    {
        var fn = Path.GetFileNameWithoutExtension(assetPath);
        var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spriteImportMode = SpriteImportMode.Multiple;

        var metaData = new List<SpriteMetaData>();

        var nWidth = grid.x;
        var nHeight = grid.y;

        var width = texture.width / nWidth;
        var height = texture.height / nHeight;

        for (int h = nHeight - 1; h >= 0; h--)
        {
            for (int w = 0; w < nWidth; w++)
            {
                var data = new SpriteMetaData();
                data.alignment = (int)alignment;
                data.name = $"{fn}_{metaData.Count}";
                data.rect = new Rect(w * width, h * height, width, height);
                metaData.Add(data);
            }
        }
        importer.spritesheet = metaData.ToArray();
        AssetDatabase.SaveAssets();
    }


    [MenuItem("Tools/Sprite/Copy Familiar/All")]
    public static void CopyFamiliar_All()
    {
        CopyFamiliar_Base();
        CopyFamiliar_Face();
        CopyFamiliar_Walk();
    }

    [MenuItem("Tools/Sprite/Copy Familiar/Base")]
    public static void CopyFamiliar_Base()
    {
        CopyFamiliar("base");
    }
    [MenuItem("Tools/Sprite/Copy Familiar/Face")]
    public static void CopyFamiliar_Face()
    {
        CopyFamiliar("face");
    }

    [MenuItem("Tools/Sprite/Copy Familiar/Walk")]
    public static void CopyFamiliar_Walk()
    {
        CopyFamiliar("walk");
    }

    static void CopyFamiliar(string type)
    {
        var fromPath = Path.Combine(Application.dataPath, "./../../../../resources/cl/Familiar");
        var toPath = Path.Combine(Application.dataPath, "./Resources/Familiar");

        foreach (var fn in Directory.GetFiles(Path.Combine(fromPath, type)))
        {
            var match = Regex.Match(fn, @"([\d]+)\w*");
            if (match == Match.Empty) continue;

            var folder = Path.Combine(toPath, match.Groups[0].ToString());
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            File.Copy(fn, Path.Combine(folder, $"{type}.png"), true);
        }
    }

}