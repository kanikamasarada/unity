using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem" , menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string itemName;
    public Sprite icon;
    [TextArea] public String description;

}
