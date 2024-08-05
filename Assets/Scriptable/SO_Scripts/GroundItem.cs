using UnityEngine;

namespace InventorySpace
{
    public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
    {
        public ItemObject item;
        public void OnBeforeSerialize()
        {
            // GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
            // EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        }

        public void OnAfterDeserialize()
        {

        }
    }
}
