using PlayerSpace;
using UnityEngine;

namespace WindowSpace
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryWindow;

        private void Awake()
        {
            _inventoryWindow.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            InputController.OnInventoryOpen += OpenInventoryWindow;
        }

        private void OpenInventoryWindow()
        {
            _inventoryWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        
        public void CloseInventoryWindow()
        {
            _inventoryWindow.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void OnDisable()
        {
            InputController.OnInventoryOpen -= OpenInventoryWindow;
        }
    }
}
