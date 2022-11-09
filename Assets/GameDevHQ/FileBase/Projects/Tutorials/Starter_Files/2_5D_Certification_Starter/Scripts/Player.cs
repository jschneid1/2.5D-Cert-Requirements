using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 6.0f, _gravity = 10.0f, _jumpHeight = 8.0f, _yVelocity;
    [SerializeField]
    private bool _grounded, _onLedge;
    private CharacterController _characterController;
    private Animator _playerAnimator;
    private PlayerAnimation _playerAnimation;

    [SerializeField]
    private Vector2 _inputMovement, _velocity, _xInput;
    [SerializeField]
    private Transform _endClimbPosition;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_characterController.enabled)
        {
            PlayerMovement();
            PlayerDirection();
        }

        if (_inputMovement.y > 0 && _onLedge)
        {
            _playerAnimator.SetTrigger("Climb Up");
        }
    }

    private void PlayerMovement()
    {
        if(_characterController.isGrounded)
        {
            _grounded = true;
            _xInput = _inputMovement;
            _velocity = _inputMovement * _playerSpeed;
        }

        else
        {
            _grounded = false;
            _velocity = _xInput * _playerSpeed;
            _yVelocity -= _gravity * Time.deltaTime;
        }

        _velocity.y = _yVelocity;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void PlayerDirection()
    {
        if (_characterController.isGrounded)
        {
            if(_xInput.x != 0)
            {
                Vector3 facing = transform.localEulerAngles;
                facing.y = _xInput.x > 0 ? 90 : 270;
                transform.localEulerAngles = facing;
            }
        }
        _playerAnimation.PlayerMove(_xInput.x);
        _playerAnimation.PlayerJump(_grounded);
    }

    private void OnMove(InputValue value)
    {
        _inputMovement = value.Get<Vector2>();
    }

    private void OnJump()
    {
        if(_characterController.isGrounded)
        {
            _yVelocity = 0;
            _yVelocity += _jumpHeight; 
        }
    }

    public void PlayerHanging(Transform handPos)
    {
        _characterController.enabled = false;
        transform.position = handPos.position;
        _playerAnimation.PlayerHangingAnimation();
        _playerAnimation.PlayerMove(0.0f);
        _onLedge = true;
    }

    public void EndClimbPosition(Transform endPos)
    {
        _endClimbPosition = endPos;
    }

    public void PlayerFinishClimb()
    {
        _onLedge = false;
        _playerAnimator.SetBool("Ledge Grab", false);
        transform.position = _endClimbPosition.position;
        _characterController.enabled = true;
        _playerAnimator.ResetTrigger("Climb Up");
    }
}
