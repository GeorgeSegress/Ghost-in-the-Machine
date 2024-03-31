using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialViewer : MonoBehaviour
{
    private UIController myController;

    public Sprite boon;
    public Sprite threat;

    private void Start()
    {
        myController = FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Special"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Special");
            //myController.AddTracker(new TrackerPack(transform, other.transform, boon));
        }
        if(other.CompareTag("Enemy"))
        {
            myController.AddTracker(new TrackerPack(transform, other.transform, threat));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Special"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("NaN");
        }
    }
}
