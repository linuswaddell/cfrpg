﻿// Defines a single item instance or a stack of items
using Newtonsoft.Json;

[System.Serializable]
public class Item
{
    public string id;
    public int quantity;

    [JsonConstructor]
    public Item() { }

    public Item (string id, int quantity)
    {
        this.id = id;
        this.quantity = quantity;
    }
    public Item (ItemData data)
    {
        id = data.ItemId;
        quantity = 1;
    }
    public ItemData GetData ()
    {
        return ContentLibrary.Instance.Items.Get(id);
    }
}