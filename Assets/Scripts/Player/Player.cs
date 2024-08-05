using InventorySpace;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private InventoryObject _inventory;

        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<GroundItem>();
            if (item)
            {
                Item invItem = new Item(item.item);
                _inventory.AddItem(invItem, 1);
                Destroy(other.gameObject);
            }
        }
    }
}