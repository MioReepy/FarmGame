using UnityEngine;

namespace InventorySpace
{
    [CreateAssetMenu(fileName = "Fruit", menuName = "Inventory System/Item/Vegetable")]
    public class VegetableObject : ItemObject
    {
        private void Awake()
        {
            type = ItemType.Vegetable;
        }
    }
}