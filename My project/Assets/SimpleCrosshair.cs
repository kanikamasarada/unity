using UnityEngine;
using UnityEngine.UI;

public class UICrosshair : MonoBehaviour
{
    public RawImage crosshairImage;    // InspectorでRawImageをセット
    [Range(2, 32)]
    public int textureSize = 8;        // 円形テクスチャの解像度
    public Color crosshairColor = Color.white;

    void Start()
    {
        // 円形テクスチャを生成して RawImage に設定
        Texture2D tex = CreateCircleTexture(textureSize, crosshairColor);
        crosshairImage.texture = tex;

        // FPS用：カーソルを非表示
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// 中心が円形、背景透明の Texture2D を生成
    /// </summary>
    private Texture2D CreateCircleTexture(int size, Color color)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;

        Color transparent = new Color(0, 0, 0, 0);
        Color[] clearPixels = new Color[size * size];
        for (int i = 0; i < clearPixels.Length; i++) clearPixels[i] = transparent;
        tex.SetPixels(clearPixels);

        float r = size / 2f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - r;
                float dy = y - r;
                if (dx * dx + dy * dy <= r * r)
                    tex.SetPixel(x, y, color);
            }
        }

        tex.Apply();
        return tex;
    }
}
