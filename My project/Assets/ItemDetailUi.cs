using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailUI : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemDescription;

    public void ShowDetail(Sprite icon, string name, string description)
    {
        itemImage.sprite = icon;
        itemName.text = name;
        itemDescription.text = description;
    }

    public void ClearDetail()
    {
        itemImage.sprite = null;
        itemName.text = "";
        itemDescription.text = "";
    }
}
