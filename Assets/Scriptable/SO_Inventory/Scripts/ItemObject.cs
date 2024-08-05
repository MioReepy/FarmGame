using System;
using UnityEngine;

namespace InventorySpace
{
    public enum ItemType
    {
        Fruit,
        Vegetable,
        RootCrop,
        Cereals,
        Tool
    }

    public abstract class ItemObject : ScriptableObject
    {
        public int ID;
        public string name;
        public Sprite uiDisplay;
        public ItemType type;
        [TextArea(15, 20)] public string description = String.Empty;

        public Item CreateItem()
        {
            Item newItem = new Item(this);
            return newItem;
        }
    }

    [Serializable]
    public class Item
    {
        public string Name;
        public int ID;

        public Item(ItemObject item)
        {
            Name = item.name;
            ID = item.ID;
        }
    }

}