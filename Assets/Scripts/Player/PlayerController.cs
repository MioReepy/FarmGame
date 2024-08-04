using CarSpace;
using UnityEngine;

namespace PlayerSpace
{
    public class PlayerController : MonoBehaviour
    {
        #region Bullet
        
        [SerializeField] private Transform _barrel;
        [SerializeField] private Transform _gunTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _bulletHitMiss = 25f;
        [SerializeField] private LayerMask _ignoreMask;
        
        #endregion

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

        #region DriveAreaParameters

        [SerializeField] private float _maxDriveDistance = 5f;

        #endregion

        #region PlayerParameters

        private CharacterController _characterController;
        private PlayerAnimator _playerAnimator;
        private bool _isGround;
        internal bool isRun;
        internal bool isWalk;
        internal bool isAim;

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
            PlayerInputController.OnJump += JumpPlayer;
            PlayerInputController.OnStartRun += StartRun;
            PlayerInputController.OnCancelRun += CancelRun;
            PlayerInputController.OnShoot += ShootGun;
            PlayerInputController.OnStratAim += StartAim;
            PlayerInputController.OnCancelAim += CancelAim;
            PlayerInputController.OnDrive += Driving;
            SelectedCar.OnGetIntoTheCar += ChangePlayerControler;
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

        public void ShootGun()
        {
            GameObject bullet = ObjectPool.SharedInstance.GetPoolesBullet();

            if (bullet != null && isAim)
            {
                bullet.transform.parent = _barrel;
                bullet.transform.position = _gunTransform.position;
                bullet.transform.rotation = _gunTransform.rotation;
                bullet.SetActive(true);

                BulletController bulletController = bullet.GetComponent<BulletController>();
                    
                Ray ray = new Ray(transform.position, _cameraTransform.forward);
            
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _ignoreMask)) 
                {
                    bulletController.Target = hit.point;
                    bulletController.Hit = true;
                }
                else
                {
                    bulletController.Target = _cameraTransform.position + _cameraTransform.forward * _bulletHitMiss;
                    bulletController.Hit = false;
                }
            }
        }

        private void StartAim()
        {
            isAim = true;
        }

        private void CancelAim()
        {
            isAim = false;
        }

        public void Driving()
        {
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit raycastHit, _maxDriveDistance))
            {
                if (raycastHit.transform.TryGetComponent(out SelectedCar car))
                {
                    car.DriveCar();
                }
            }
        }

        private void ChangePlayerControler()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            PlayerInputController.OnJump -= JumpPlayer;
            PlayerInputController.OnStartRun -= StartRun;
            PlayerInputController.OnCancelRun -= CancelRun;
            PlayerInputController.OnShoot -= ShootGun;
            PlayerInputController.OnStratAim -= StartAim;
            PlayerInputController.OnCancelAim -= CancelAim;
            PlayerInputController.OnDrive -= Driving;
            SelectedCar.OnGetIntoTheCar -= Driving;
        }
    }
}