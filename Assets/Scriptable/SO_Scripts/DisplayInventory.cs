using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace InventorySpace
{
    public class DisplayInventory : MonoBehaviour
    {
        private MouseItem _mouseItem;
        
        [SerializeField] private GameObject _inventoryCellPrefab;
        [SerializeField] private InventoryObject _inventory;

        [SerializeField] private int X_Start;
        [SerializeField] private int Y_Start;
        [SerializeField] private int X_SpaceBetweenItem;
        [SerializeField] private int Y_SpaceBetweenItem;
        [SerializeField] private int NumberOfColums;
 
        private Dictionary<GameObject, InventorySlot> _itemsDisplayed;

        private void Start()
        {
            _mouseItem = new MouseItem();
            CreateSlots();
        }

        public void CreateSlots()
        {
            _itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

            for (int i = 0; i < _inventory.Container.Items.Length; i++)
            {
                var obj = Instantiate(_inventoryCellPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                
                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
                AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragBegin(obj); });
                AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
                AddEvent(obj, EventTriggerType.Drag, delegate { OnDragged(obj); });
            }
        }

        private void OnEnter(GameObject obj)
        {
            _mouseItem.hoverObj = obj;

            if (_itemsDisplayed.ContainsKey(obj))
            {
                _mouseItem.hoverItem = _itemsDisplayed[obj];
            }
        }

        private void OnExit(GameObject obj)
        {
            Debug.Log("Цвет меняем на старый");
            _mouseItem.hoverObj = null;
        }

        private void OnDragBegin(GameObject obj)
        {
            Debug.Log("Начало перетаскивания");        
        }

        private void OnDragEnd(GameObject obj)
        {
            Debug.Log("Окончание перетаскивания");        
        }

        private void OnDragged(GameObject obj)
        {
            Debug.Log("Перетаскивание");        
        }

        private Vector3 GetPosition(int i)
        {
            return new Vector3(X_Start + (X_SpaceBetweenItem * (i % NumberOfColums)),
                Y_Start + (-Y_SpaceBetweenItem * (i / NumberOfColums)), 0f);
        }

        private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            EventTrigger.Entry eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }
    }
    
    public class MouseItem
    {
        public GameObject obj;
        public GameObject hoverObj;
        public InventorySlot hoverItem;
    }
}