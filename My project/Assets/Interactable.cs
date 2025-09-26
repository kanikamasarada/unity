using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Item item;  // このオブジェクトに紐づくアイテム

    public void OnInteract()
    {
        // 拾ったらゲーム内から消す
        Destroy(gameObject);
    }
}
