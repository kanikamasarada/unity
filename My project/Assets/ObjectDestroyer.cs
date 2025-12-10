using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [Header("削除する対象オブジェクト")]
    public GameObject targetObject;

    [Header("開始時に削除するか")]
    public bool destroyOnStart = true;

    /// <summary>
    /// MonoBehaviour の Start で自動削除する
    /// </summary>
    void Start()
    {
        if (destroyOnStart)
        {
            DestroyTarget();
        }
    }

    /// <summary>
    /// 外部から呼び出し可能な 削除関数
    /// </summary>
    public void DestroyTarget()
    {
        if (targetObject != null)
        {
            Destroy(targetObject);
            Debug.Log($"ObjectDestroyer: {targetObject.name} を削除しました");
        }
        else
        {
            Debug.LogWarning("ObjectDestroyer: targetObject が設定されていません");
        }
    }
}
