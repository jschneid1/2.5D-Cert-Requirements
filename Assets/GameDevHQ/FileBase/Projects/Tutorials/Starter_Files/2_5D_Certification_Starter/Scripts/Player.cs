using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 6.0f, _gravity = 10.0f, _jumpHeight = 8.0f, _yVelocity, _xVelocity;
    [SerializeField]
    private bool _grounded, _onLedge, _onLadder, _canClimb;
    private CharacterController _characterController;
    private Animator _playerAnimator;
    private PlayerAnimation _playerAnimation;

    [SerializeField]
    private Vector2 _inputMovement, _velocity;
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

        if(Mathf.Abs(_inputMovement.y) > 0 && _canClimb && _grounded)
        {
            _playerAnimator.SetBool("Can Climb Ladder", true);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.001f, transform.position.z);
            _xVelocity = 0f;
            _yVelocity = 0f;
            _onLadder = true;
            _grounded = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.CompareTag("Ladder"))
            {
                Debug.Log("Player Character Controller hit Ladder");
                _canClimb = true;
            }
    }
    private void PlayerMovement()
    {
        if (_onLadder)
        {
            _xVelocity = 0f;
            _velocity.y = _inputMovement.y * _playerSpeed;
        }

        else if (_onLadder == false)
        {
            if (_characterController.isGrounded)
            {
                _grounded = true;
                _xVelocity = _inputMovement.x;
                _velocity = _inputMovement * _playerSpeed;
            }

            else
            {
                _grounded = false;
                _velocity.x = _xVelocity * _playerSpeed;
                _yVelocity -= _gravity * Time.deltaTime;
            }

            _velocity.y = _yVelocity;
        }

        


        _characterController.Move(_velocity * Time.deltaTime);

    }

    private void PlayerDirection()
    {
        if (_characterController.isGrounded)
        {
            if(_xVelocity != 0)
            {
                Vector3 facing = transform.localEulerAngles;
                facing.y = _xVelocity > 0 ? 90 : 270;
                transform.localEulerAngles = facing;
            }
        }
        _playerAnimation.PlayerMove(_xVelocity);
        _playerAnimation.PlayerJump(_grounded);

        if(_onLadder)
        {
            _playerAnimation.PlayerMove(_inputMovement.y);
        }
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
        _onLadder = false;
        _canClimb = false;
        _playerAnimator.SetBool("Can Climb Ladder", false);
        _playerAnimator.SetBool("Ledge Grab", false);
        transform.position = _endClimbPosition.position;
        _characterController.enabled = true;
        _playerAnimator.ResetTrigger("Climb Up");
    }

    public void CanClimb()
    {
        _canClimb = true;
    }

    public void OffLadder()
    {
        _grounded = true;
        _onLadder = false;
        _canClimb = false;
        _playerAnimator.SetBool("Can Climb Ladder", false);
    }
}
