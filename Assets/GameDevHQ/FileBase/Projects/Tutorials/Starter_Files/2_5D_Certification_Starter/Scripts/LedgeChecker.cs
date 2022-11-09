using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    [SerializeField]
    private Transform _handPositionTransform, _endClimbPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ledge_Checker"))
            {
            Player player = other.GetComponentInParent<Player>();
            if (player == null)
            {
                Debug.Log("There is no Player Script!!!");
            }
            player.PlayerHanging(_handPositionTransform);
            player.EndClimbPosition(_endClimbPosition);
            }
    }
}
