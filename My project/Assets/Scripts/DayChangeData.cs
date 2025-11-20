using UnityEngine;

[System.Serializable]
public class DayChangeData
{
    public int day;

    public Vector3 newPosition;
    public bool active;

    public Texture newTexture;         // 通常のテクスチャー
    public Texture distanceTexture;    // 距離内に入ったときのテクスチャー
    public float distanceRange = 3f;   // 距離判定
}
