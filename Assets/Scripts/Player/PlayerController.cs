using CarSpace;
using UnityEngine;

namespace PlayerSpace
{
    public class PlayerController : MonoBehaviour
    {
        #region MoveSettings

        [SerializeField] private float _playerWalkSpeed = 1f;
        [SerializeField] private float _playerRunSpeed = 2f;
        [SerializeField] private float _rotationSpeed = 2f;
        [SerializeField] private float _animSmoothTime = 0.2f;
        [SerializeField] private float _jumpHeight = 1f;

        private Vector3 _moveInput;
        private Vector3 _move;
        internal Vector2 currentBlendAnim;
        private Vector2 _animVelosity;
        private float _currentSpeed;
        private Vector3 _playerVelosity;

        #endregion

        #region PlayerParameters

        [SerializeField] private Transform _cameraTransform;
        private CharacterController _characterController;
        private PlayerAnimator _playerAnimator;
        private bool _isGround;
        internal bool isRun;
        internal bool isWalk;

        #endregion

        #region Gravity

        [SerializeField] private float _gravityForce = 0.5f;
        private const float _gravityValue = -9.81f;

        #endregion
        
        public Vector2 MoveInput
        {
            set
            {
                _moveInput.x = value.x;
                _moveInput.y = value.y;
            }
        }
        
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerVelosity = _characterController.transform.position;
            _currentSpeed = _playerWalkSpeed;
        }

        private void OnEnable()
        {
            InputController.OnJump += JumpPlayer;
            InputController.OnStartRun += StartRun;
            InputController.OnCancelRun += CancelRun;
        }

        private void Update()
        {
            GroundCheck();
            MovePlayer();
            RotateToDirection();
            ApplyGravity();
        }

        private void GroundCheck()
        {
            _isGround = _characterController.isGrounded;

            if (!_isGround && _playerVelosity.y < 0)
            {
                _playerVelosity.y = 0;
            }
        }

        private void ApplyGravity()
        {
            if (!_isGround && gameObject.transform.position.y > 0f)
            {
                _characterController.Move(Vector3.up * _gravityValue * _gravityForce * Time.deltaTime);
            }
        }

        private void MovePlayer()
        {
            currentBlendAnim = Vector2.SmoothDamp(currentBlendAnim, _moveInput, ref _animVelosity, _animSmoothTime);
            _move = new Vector3(currentBlendAnim.x, 0f, currentBlendAnim.y);
            _move = _cameraTransform.right * _moveInput.x + _cameraTransform.forward * _moveInput.y;
            _characterController.Move(_move * _currentSpeed * Time.deltaTime);
        }

        private void JumpPlayer()
        {
            if (_isGround)
            {
                _playerVelosity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
                _characterController.Move(Vector3.up * _playerVelosity.y * _gravityForce * Time.deltaTime);
                _playerAnimator.JumpAnimation();
            }
        }

        private void RotateToDirection()
        {
            if (_moveInput != Vector3.zero)
            {
                Quaternion rotation = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void StartRun()
        {
            _currentSpeed = _playerRunSpeed;
            isRun = true;
        }

        private void CancelRun()
        {
            _currentSpeed = _playerWalkSpeed;
            isRun = false;
        }
        
        private void ChangePlayerControler()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            InputController.OnJump -= JumpPlayer;
            InputController.OnStartRun -= StartRun;
            InputController.OnCancelRun -= CancelRun;
        }
    }
}