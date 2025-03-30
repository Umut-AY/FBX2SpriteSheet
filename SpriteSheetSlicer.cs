using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class SpriteSheetSlicer
{
    [MenuItem("Tools/Slice Sprite Sheet (Auto)")]
    public static void SliceSpriteSheet()
    {
        string path = "Assets/SpriteSheets/sprite_sheet_grid.png";

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError("❌ Sprite sheet bulunamadı veya yanlış path!");
            return;
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.filterMode = FilterMode.Point;
        importer.maxTextureSize = 8192;
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (tex == null)
        {
            Debug.LogError("❌ Texture2D yüklenemedi!");
            return;
        }

        int texWidth = tex.width;
        int texHeight = tex.height;

        int cellWidth = 128;
        int cellHeight = 128;

        int columns = texWidth / cellWidth;
        int rows = texHeight / cellHeight;

        Debug.Log($"🧩 Texture size: {texWidth}x{texHeight}, Kare boyutu: {cellWidth}x{cellHeight}");
        Debug.Log($"🔢 Slice sonucu: {columns} sütun, {rows} satır → {columns * rows} frame");

        List<SpriteMetaData> metaDataList = new List<SpriteMetaData>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                SpriteMetaData smd = new SpriteMetaData();
                smd.name = $"frame_{y * columns + x}";
                smd.rect = new Rect(
                    x * cellWidth,
                    texHeight - ((y + 1) * cellHeight),
                    cellWidth,
                    cellHeight
                );
                smd.alignment = (int)SpriteAlignment.Center;
                smd.pivot = new Vector2(0.5f, 0.5f);

                metaDataList.Add(smd);
            }
        }

        importer.spritesheet = metaDataList.ToArray();
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();

        Debug.Log($"✅ Sprite sheet slice edildi: {columns}x{rows} → {columns * rows} frame");
    }

    [MenuItem("Tools/Downscale Sprite Sheet to Pixel Art (64x64)")]
    public static void DownscaleSpriteSheet()
    {
        string inputPath = "Assets/SpriteSheets/sprite_sheet_grid.png";
        string outputPath = "Assets/SpriteSheets/pixelart_sheet.png";

        Texture2D original = AssetDatabase.LoadAssetAtPath<Texture2D>(inputPath);
        if (original == null)
        {
            Debug.LogError("❌ Orijinal sprite sheet yüklenemedi.");
            return;
        }

        Texture2D downscaled = Downscale(original, 64, 64);
        byte[] pngData = downscaled.EncodeToPNG();

        File.WriteAllBytes(outputPath, pngData);
        Debug.Log("✅ Pixel art sprite sheet oluşturuldu: " + outputPath);
        AssetDatabase.Refresh();
    }

    public static Texture2D Downscale(Texture2D source, int targetWidth, int targetHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
        rt.filterMode = FilterMode.Point;

        RenderTexture.active = rt;
        Graphics.Blit(source, rt);

        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }
}
