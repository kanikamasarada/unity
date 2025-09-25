using UnityEngine;
using UnityEngine.UI;

public class UICrosshair : MonoBehaviour
{
    public RawImage crosshairImage;    // Inspector��RawImage���Z�b�g
    [Range(2, 32)]
    public int textureSize = 8;        // �~�`�e�N�X�`���̉𑜓x
    public Color crosshairColor = Color.white;

    void Start()
    {
        // �~�`�e�N�X�`���𐶐����� RawImage �ɐݒ�
        Texture2D tex = CreateCircleTexture(textureSize, crosshairColor);
        crosshairImage.texture = tex;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

 
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
