using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    private Animator myAnim;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        GetComponent<AudioSource>().Play();
        myAnim.SetTrigger("Trigger");
    }
}
