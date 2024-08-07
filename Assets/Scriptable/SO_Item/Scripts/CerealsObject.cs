using UnityEngine;

namespace InventorySpace
{
    [CreateAssetMenu(fileName = "Cereals", menuName = "Inventory System/Item/Cereals")]
    public class CerealsObject : ItemObject
    {
        private void Awake()
        {
            type = ItemType.Cereals;
        }
    }
}