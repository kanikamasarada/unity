// Interactable.cs
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string itemName;
    public GameObject prefab;   // ”z’u‚É¶¬‚·‚éŒ³

    public void OnInteract()
    {
        // ‚±‚±‚Å‚Í”ñ•\¦‚É‚µ‚ÄInventory‚É“n‚·‚¾‚¯
        gameObject.SetActive(false);
    }
}
