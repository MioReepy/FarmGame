using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        private void Update()
        {
            UpdateSlots();
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
                
                _itemsDisplayed.Add(obj, _inventory.Container.Items[i]);
            }
        }

        public void UpdateSlots()
        {
            foreach (KeyValuePair<GameObject, InventorySlot> invSlot in _itemsDisplayed)
            {
                if (invSlot.Value.ID >= 0)
                {
                    invSlot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                        _inventory.DataBase.GetItem[invSlot.Value.Item.ID].uiDisplay;
                    invSlot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color =
                        new Color(1, 1, 1, 1);
                    invSlot.Key.GetComponentInChildren<TextMeshProUGUI>().text =
                        invSlot.Value.Amount == 1 ? "" : invSlot.Value.Amount.ToString("n0");
                }
                else
                {
                    invSlot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                    invSlot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color =
                        new Color(1, 1, 1, 0);
                    invSlot.Key.GetComponentInChildren<TextMeshProUGUI>().text = String.Empty;
                }

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
            _mouseItem.hoverObj = null;
            _mouseItem.hoverItem = null;
        }

        private void OnDragBegin(GameObject obj)
        {
            var mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50f, 50f);
            mouseObject.transform.SetParent(transform.parent);

            if (_itemsDisplayed[obj].ID >= 0)
            {
                var img = mouseObject.AddComponent<Image>();
                img.sprite = _inventory.DataBase.GetItem[_itemsDisplayed[obj].ID].uiDisplay;
                img.raycastTarget = false;
            }

            _mouseItem.obj = mouseObject;
            _mouseItem.item = _itemsDisplayed[obj];
        }

        private void OnDragEnd(GameObject obj)
        {
            if (_mouseItem.hoverObj)
            {
                _inventory.MoveItem(_itemsDisplayed[obj], _itemsDisplayed[_mouseItem.hoverObj]);
            }
            else
            {
                _inventory.RemoveItem(_itemsDisplayed[obj].Item);
            }
            
            Destroy(_mouseItem.obj);
            _mouseItem.item = null;
        }

        private void OnDragged(GameObject obj)
        {
            if (_mouseItem.obj != null)
            {
                _mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition; 
            }
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
        public InventorySlot item;
    }
}