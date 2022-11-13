using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Lift_Movement : MonoBehaviour
{
    [SerializeField]
    private Transform[] _liftLevel;
    //private Transform _levelFour, _levelThree, _levelTwo, _levelOne, _groundLevel;
    private Transform _nextLiftLevel;
    [SerializeField]
    private bool _hasBeenTop, _hasBeenBottom;
    [SerializeField]
    private float _liftSpeed = 3;
    [SerializeField]
    private int _currentTravelToLevel, _liftArrayLenth;

    // Start is called before the first frame update
    private void Start()
    {
        _liftArrayLenth = _liftLevel.Length;
        _currentTravelToLevel = _liftArrayLenth - 2;
        _nextLiftLevel = _liftLevel[_currentTravelToLevel];
        _hasBeenTop = true;
    }

    // Update is called once per frame
    private void Update()
    {
        LiftMovement();
    }

    private void LiftMovement()
    {
       if (transform.position.y == _nextLiftLevel.position.y)
        {
            StartCoroutine(LiftPauseRoutine());
        }

        transform.position = Vector3.MoveTowards(transform.position, _nextLiftLevel.position, _liftSpeed * Time.deltaTime);
    }

    IEnumerator LiftPauseRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        Debug.Log("5 seconds are up");
        LiftPauseRoutineStop();
        StopAllCoroutines();
    }

    private void LiftPauseRoutineStop()
    {
        if (_hasBeenTop && _currentTravelToLevel > 0)
        {
            _currentTravelToLevel -= 1;
        }

        else
        {
            _hasBeenTop = false;
            _currentTravelToLevel += 1;
        }

        if(_hasBeenTop == false && _currentTravelToLevel == _liftArrayLenth - 1)
        {
            _hasBeenTop = true;
        }
        _nextLiftLevel = _liftLevel[_currentTravelToLevel];
    }

}
