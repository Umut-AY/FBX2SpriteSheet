using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class SpriteSheetToAnimation
{
    [MenuItem("Tools/Create Animation From Sprite Sheet")]
    public static void CreateAnimationClip()
    {
        string spriteSheetPath = "Assets/SpriteSheets/sprite_sheet_grid.png"; // <- kendi sprite sheet yolun
        string outputClipPath = "Assets/Animations/idle.anim"; // <- kaydedilecek animasyon

        // Sprite'larý al
        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(spriteSheetPath);
        if (sprites.Length == 0)
        {
            Debug.LogError("Sprite bulunamadý. Sprite sheet sliced mý?");
            return;
        }

        // FPS ve süre
        float frameRate = 12f;
        float frameTime = 1f / frameRate;

        // Keyframe dizisi oluþtur
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i * frameTime,
                value = (Sprite)sprites[i]
            };
        }

        // Animasyon klibi oluþtur
        AnimationClip clip = new AnimationClip();
        clip.frameRate = frameRate;

        // Sprite renderer'ýn sprite'ýna baðla
        EditorCurveBinding spriteBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        // Keyframe'leri klibe uygula
        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyframes);

        // Animasyon dosyasýný kaydet
        Directory.CreateDirectory("Assets/Animations");
        AssetDatabase.CreateAsset(clip, outputClipPath);
        AssetDatabase.SaveAssets();

        Debug.Log("Animasyon oluþturuldu: " + outputClipPath);
    }
}
