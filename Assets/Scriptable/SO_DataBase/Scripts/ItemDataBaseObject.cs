using System.Collections.Generic;
using UnityEngine;

namespace InventorySpace
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Database")]
    public class ItemDataBaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemObject[] Items;
        internal List<ItemObject> GetItem = new List<ItemObject>();
        
        
        public void OnBeforeSerialize()
        {
            GetItem = new List<ItemObject>();
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].ID = i;
                GetItem.Add(Items[i]);
            }
        }
    }
}
