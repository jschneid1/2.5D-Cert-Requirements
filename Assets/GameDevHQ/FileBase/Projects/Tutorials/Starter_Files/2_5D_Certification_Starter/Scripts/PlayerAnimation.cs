using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator = GetComponentInChildren<Animator>();
    }

    public void PlayerMove(float move)
    {
        _playerAnimator.SetFloat("Speed", Mathf.Abs(move));
    }

    public void PlayerJump(bool grounded)
    {
        _playerAnimator.SetBool("Grounded", grounded);
    }

    public void PlayerHangingAnimation()
    {
        _playerAnimator.SetBool("Ledge Grab", true);
    }
}
