using UnityEngine;

namespace InventorySpace
{
    [CreateAssetMenu(fileName = "Fruit", menuName = "Inventory System/Item/Fruit")]
    public class FruitObject : ItemObject
    {
        private void Awake()
        {
            type = ItemType.Fruit;
        }
    }
}