using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickup : MonoBehaviour
{
    public Note myMessage;

    private void Start()
    {
        string h = "";
        foreach(char c in myMessage.text)
        {
            if (c != '\\')
                h += c;
            else
                h += '\n';
        }
        myMessage.text = h;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().NoteForDeck(myMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            other.GetComponent<Player>().ClearDeck();
    }
}

[System.Serializable]
public class Note
{
    public string name;
    public string text;
    public AudioClip audio;
    public GameObject myObj;
    public int number;

    public Note (string myName, string myText, AudioClip myAudio, int myNum)
    {
        name = myName;
        string h = "";
        foreach (char c in myText)
        {
            if (c != '\\')
                h += c;
            else
                h += '\n';
        }
        text = h;
        audio = myAudio;
        myObj = null;
        number = myNum;
    }

    public Note()
    {
        name = "";
        text = "";
        audio = null;
        myObj = null;
        number = -1;
    }
}