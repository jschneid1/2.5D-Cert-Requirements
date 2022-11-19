using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if(other.CompareTag("Player_Ladder_Bottom"))
        {
            player.OffLadder();
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (other.CompareTag("Player_Ladder_Top"))
        {
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Player player=other.GetComponentInParent<Player>();

        if(other.CompareTag("Player_Ladder_Bottom"))
        {
            
        }
    }*/
}
