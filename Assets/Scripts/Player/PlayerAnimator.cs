using UnityEngine;

namespace PlayerSpace
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private float _animationPlayTransition = 0.1f;
        
        private Animator _playerAnimator;
        private PlayerController _playerController;
        private int _moveXAnimationParametrId;
        private int _moveYAnimationParametrId;
        private int _jumpAnimation;

        private void Awake()
        {
            _moveXAnimationParametrId = Animator.StringToHash("MovementX");
            _moveYAnimationParametrId = Animator.StringToHash("MovementY");
            _jumpAnimation = Animator.StringToHash("Jumping");
        }

        private void Start()
        {
            _playerAnimator = GetComponent<Animator>();
            _playerController = GetComponent<PlayerController>();
        }
        
        private void FixedUpdate()
        {
            _playerAnimator.SetFloat(_moveXAnimationParametrId, _playerController.currentBlendAnim.x);
            _playerAnimator.SetFloat(_moveYAnimationParametrId, _playerController.currentBlendAnim.y);
            _playerAnimator.SetBool("isRun", _playerController.isRun);
            _playerAnimator.SetBool("isAim", _playerController.isAim);
            _playerAnimator.SetBool("isWalk", _playerController.isWalk);
        }
        
        internal void JumpAnimation()
        {
            _playerAnimator.CrossFade(_jumpAnimation, _animationPlayTransition);
        }
    }
}