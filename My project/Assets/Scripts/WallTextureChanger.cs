using UnityEngine;

public class WallTextureChanger : MonoBehaviour
{
    public Texture[] wallTextures;  // 壁に使うテクスチャ（インスペクターに設定）
    public Renderer wallRenderer;   // 壁のRenderer（インスペクターでアタッチ）

    private int currentDay = 0;

    void Start()
    {
        UpdateWallTexture();
    }

    // 日付が進んだ時に呼ぶメソッド
    public void NextDay()
    {
        currentDay++;
        if (currentDay >= wallTextures.Length)
        {
            currentDay = 0; // ループさせたい場合
        }
        UpdateWallTexture();
    }

    void UpdateWallTexture()
    {
        if (wallRenderer != null && wallTextures.Length > 0)
        {
            wallRenderer.material.mainTexture = wallTextures[currentDay];
        }
    }
}

