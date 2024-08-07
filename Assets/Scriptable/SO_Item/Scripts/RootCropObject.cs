using UnityEngine;

namespace InventorySpace
{
    [CreateAssetMenu(fileName = "RootCrop", menuName = "Inventory System/Item/RootCrop")]
    public class RootCropObject : ItemObject
    {
        private void Awake()
        {
            type = ItemType.RootCrop;
        }
    }
}