using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class GridSpriteSheetBuilder
{
    [MenuItem("Tools/Generate Sprite Sheet (Grid)")]
    public static void BuildGridSheet()
    {
        string inputPath = Application.dataPath + "/CapturedFrames";
        string outputPath = Application.dataPath + "/SpriteSheets/sprite_sheet_grid.png";

        string[] files = Directory.GetFiles(inputPath, "*.png");
        System.Array.Sort(files); // 🔧 Sıralama eklendi

        if (files.Length == 0)
        {
            Debug.LogWarning("Frame bulunamadı.");
            return;
        }

        List<Texture2D> frames = new List<Texture2D>();

        foreach (string file in files)
        {
            byte[] data = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(data);
            frames.Add(tex);
        }

        int frameWidth = frames[0].width;
        int frameHeight = frames[0].height;

        int columns = Mathf.CeilToInt(Mathf.Sqrt(frames.Count));
        int rows = Mathf.CeilToInt((float)frames.Count / columns);

        Texture2D spriteSheet = new Texture2D(columns * frameWidth, rows * frameHeight, TextureFormat.RGBA32, false);

        for (int i = 0; i < frames.Count; i++)
        {
            int x = (i % columns) * frameWidth;
            int y = (rows - 1 - i / columns) * frameHeight;

            spriteSheet.SetPixels(x, y, frameWidth, frameHeight, frames[i].GetPixels());
        }

        spriteSheet.Apply();

        Directory.CreateDirectory(Application.dataPath + "/SpriteSheets");
        File.WriteAllBytes(outputPath, spriteSheet.EncodeToPNG());

        Debug.Log($"Sprite Sheet oluşturuldu: {outputPath}");
        AssetDatabase.Refresh();
    }
}