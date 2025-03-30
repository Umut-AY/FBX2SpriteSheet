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

        // Sprite'lar� al
        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(spriteSheetPath);
        if (sprites.Length == 0)
        {
            Debug.LogError("Sprite bulunamad�. Sprite sheet sliced m�?");
            return;
        }

        // FPS ve s�re
        float frameRate = 12f;
        float frameTime = 1f / frameRate;

        // Keyframe dizisi olu�tur
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i * frameTime,
                value = (Sprite)sprites[i]
            };
        }

        // Animasyon klibi olu�tur
        AnimationClip clip = new AnimationClip();
        clip.frameRate = frameRate;

        // Sprite renderer'�n sprite'�na ba�la
        EditorCurveBinding spriteBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        // Keyframe'leri klibe uygula
        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyframes);

        // Animasyon dosyas�n� kaydet
        Directory.CreateDirectory("Assets/Animations");
        AssetDatabase.CreateAsset(clip, outputClipPath);
        AssetDatabase.SaveAssets();

        Debug.Log("Animasyon olu�turuldu: " + outputClipPath);
    }
}
