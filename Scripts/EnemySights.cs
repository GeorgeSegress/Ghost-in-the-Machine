using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemySights : MonoBehaviour
{
    public bool visualFollower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().SawPlayer(other.transform);
            visualFollower = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(visualFollower && other.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().UnsawPlayer();
            visualFollower = false;
        }
    }
}
