using UnityEngine;
using UnityEngine.UI;

public class SelectedItemDisplay : MonoBehaviour
{
    public static SelectedItemDisplay Instance;
    public Image displayImage;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowItem(ItemData item)
    {
        if (item == null || displayImage == null) return;
        displayImage.sprite = item.icon;
        displayImage.enabled = true;
    }
}
