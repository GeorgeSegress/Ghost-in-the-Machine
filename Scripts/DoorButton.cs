using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReceiverType
{
    Door
}

public class DoorButton : MonoBehaviour
{
    public ReceiverType myReceiverType;
    public GameObject myReceiver;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            myReceiver.GetComponent<Receiver>().OpenDoor();
    }
}
