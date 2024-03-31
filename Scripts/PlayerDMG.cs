using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDMG : MonoBehaviour
{
    public float myDamage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().HarmPlayer(myDamage);
        }
    }
}
