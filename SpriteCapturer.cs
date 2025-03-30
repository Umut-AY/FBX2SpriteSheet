using UnityEngine;
using System.IO;

public class SpriteCapturer : MonoBehaviour
{
    public Camera captureCamera;
    public RenderTexture renderTexture;
    public int frameCount = 10;
    public float captureInterval = 0.1f; // saniyede 10 kare
    private int currentFrame = 0;

    void Start()
    {
        StartCoroutine(CaptureFrames());
    }

    System.Collections.IEnumerator CaptureFrames()
    {
        while (currentFrame < frameCount)
        {
            yield return new WaitForSeconds(captureInterval);

            RenderTexture.active = renderTexture;
            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + $"/Frame_{currentFrame:D3}.png", bytes);
            Debug.Log($"Captured Frame {currentFrame}");

            currentFrame++;
        }

        RenderTexture.active = null;
    }
}
