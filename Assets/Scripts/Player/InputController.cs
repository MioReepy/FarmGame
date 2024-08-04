using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSpace
{
    public class InputController : MonoBehaviour
    {
        private PlayerController _playerController;
        
        #region InputAction

        private PlayerInput _playerInputController;
        private InputAction _actionMove;
        private InputAction _actionRun;
        private InputAction _actionJump;
        private InputAction _actionShoot;
        private InputAction _actionAim;
        private InputAction _actionDrive;

        #endregion

        public delegate void Interact();
        public static event Interact OnJump;
        public static event Interact OnStartRun;
        public static event Interact OnCancelRun;
        public static event Interact OnShoot;
        public static event Interact OnStratAim;
        public static event Interact OnCancelAim;
        public static event Interact OnDrive;
        
        private void Awake()
        {
            _playerInputController = GetComponent<PlayerInput>();
            _playerController = GetComponent<PlayerController>();
            
            _actionMove = _playerInputController.actions["Move"];
            _actionRun = _playerInputController.actions["Run"];
            _actionJump = _playerInputController.actions["Jump"];
            _actionShoot = _playerInputController.actions["Shoot"];
            _actionAim = _playerInputController.actions["Aim"];
            _actionDrive = _playerInputController.actions["Drive"];

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _actionRun.canceled += _ => CancelRun();
            _actionRun.performed += _ => StartRun();
            _actionShoot.performed += _ => Shooting();
            _actionAim.performed += _ => StartAim();
            _actionAim.canceled += _ => CancelAim();
            _actionDrive.performed += _ => Driveing();
        }

        private void Update()
        {
            Moving();
            Jumping();
        }

        private void Moving()
        {
            Vector2 input = _actionMove.ReadValue<Vector2>();
            _playerController.MoveInput = input;

            if (input != Vector2.zero)
            {
                _playerController.isWalk = true;
            }
            else
            {
                _playerController.isWalk = false;
            }
        }

        private void StartRun()
        {
            OnStartRun?.Invoke();
        }

        private void CancelRun()
        {
            OnCancelRun?.Invoke();
        }

        private void Jumping()
        {
            if (_actionJump.triggered)
            {
                OnJump?.Invoke();
            }
        }

        private void Shooting()
        {
            OnShoot?.Invoke();
        }

        private void StartAim()
        {
            OnStratAim?.Invoke();
        }

        private void CancelAim()
        {
            OnCancelAim?.Invoke();
        }

        private void Driveing()
        {
            OnDrive?.Invoke();
        }

        private void OnDisable()
        {
            _actionRun.canceled -= _ => CancelRun();
            _actionRun.performed -= _ => StartRun();
            _actionShoot.performed -= _ => Shooting();
            _actionAim.performed -= _ => StartAim();
            _actionAim.canceled -= _ => CancelAim();
            _actionDrive.performed -= _ => Driveing();
        }
    }
}