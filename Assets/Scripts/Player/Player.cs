using InventorySpace;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private InventoryObject _inventory;

        private void OnTriggerStay(Collider other)
        {
            var item = other.GetComponent<GroundItem>();
            var maturation = other.GetComponent<PlantStage>();
            
            if (item && maturation._isReady)
            {
                Item invItem = new Item(item.item);
                _inventory.AddItem(invItem, 1);
                Destroy(other.gameObject);
            }
        }
    }
}