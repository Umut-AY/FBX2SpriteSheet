using UnityEngine;
using System.IO;

public class TransparentFrameCapturer : MonoBehaviour
{
    public Camera captureCamera;
    public RenderTexture renderTexture;
    public int frameCount = 30;
    public float captureInterval = 0.1f;

    private int currentFrame = 0;

    void Start()
    {
        StartCoroutine(CaptureFrames());
    }

    System.Collections.IEnumerator CaptureFrames()
    {
        yield return new WaitForEndOfFrame(); // bir frame geçmesi için

        while (currentFrame < frameCount)
        {
            yield return new WaitForSeconds(captureInterval);

            RenderTexture.active = renderTexture;
            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            byte[] pngBytes = tex.EncodeToPNG();
            string filePath = Application.dataPath + $"/CapturedFrames/frame_{currentFrame:D3}.png";

            Directory.CreateDirectory(Application.dataPath + "/CapturedFrames");
            File.WriteAllBytes(filePath, pngBytes);
            Debug.Log($"Saved: {filePath}");

            currentFrame++;
        }

        RenderTexture.active = null;
    }
}
