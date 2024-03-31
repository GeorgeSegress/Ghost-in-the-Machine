using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject fuelAlert;
    public GameObject fuelOut;
    public GameObject gotKilled;
    public float runLossRate;
    public float walkRate;
    public float idleRate;
    public Transform myRefuelSpawn;

    public float healthPoints = 100;
    public float fuelPoints = 100;
    private FirstPersonMovement myMove;
    private UIController myUI;
    private Note noteDeck = new Note();
    private Note[] myNote = new Note[11];
    private bool openNotes;
    private GasPump myPump;

    private GasPump deckedPump;
    private Transform deckedSpawn;

    private void Start()
    {
        myMove = GetComponent<FirstPersonMovement>();
        myUI = FindObjectOfType<UIController>();
        for(int i = 0; i < myNote.Length; i++)
        {
            myNote[i] = new Note();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceHit();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            NotesHit();
        }
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (myMove.canMove)
        {
            if (myMove.curState == RunState.Running)
                fuelPoints -= Time.deltaTime * runLossRate;
            else if (myMove.curState == RunState.Walking)
                fuelPoints -= Time.deltaTime * walkRate;
            else
                fuelPoints -= Time.deltaTime * idleRate;

            fuelAlert.SetActive(fuelPoints <= 20 && fuelPoints > 0);
            fuelOut.SetActive(fuelPoints <= 0);
            if (fuelPoints <= 0)
                HarmPlayer(101);
            myUI.UpdateFuel(fuelPoints);
        }
    }

    public void Refuel()
    {
        fuelPoints = 100;
    }

    public void HarmPlayer(float dmg)
    {
        healthPoints -= dmg;
        if(dmg == 101)
        {
            fuelAlert.SetActive(false);
            fuelOut.SetActive(true);
            myMove.canMove = false;
        }
        else if (healthPoints <= 0)
        {
            gotKilled.SetActive(true);
            myMove.canMove = false;
        }
    }

    public void SpaceHit()
    {
        if (!myMove.canMove)
        {
            Revive();
            return;
        }
        if (deckedSpawn != null)
        {
            UsePump();
        }
        if (noteDeck.text != "")
        {
            PickupNote();
        }
    }

    public void NotesHit()
    {
        if (!openNotes)
        {
            myUI.DisplayNotes(myNote);
            openNotes = true;
        }
        else
        {
            myUI.CloseNotes();
            openNotes = false;
        }
    }

    public void Revive()
    {
        SceneManager.LoadScene(0);
        transform.position = myRefuelSpawn.position;
        healthPoints = 100;
        fuelPoints = 100;
        fuelOut.SetActive(false);
        gotKilled.SetActive(false);
        ClearDeck();
        myMove.canMove = true;
    }

    public void NoteForDeck(Note newNote)
    {
        noteDeck = newNote;
        myUI.NoteAvailable(true);
    }

    public void ClearDeck()
    {
        noteDeck = new Note();
        myUI.NoteAvailable(false);
    }

    public void PickupNote()
    {
        myUI.ReceiveText(noteDeck.text);
        myNote[noteDeck.number] = noteDeck;
        noteDeck.myObj.SetActive(false);
        ClearDeck();
    }

    public void QueuePump(GasPump gp, Transform sp)
    {
        myUI.QueuePump();
        deckedPump = gp;
        deckedSpawn = sp;
    }

    public void UsePump()
    {
        fuelPoints = 100;
        healthPoints = 100;
        myPump = deckedPump;
        myRefuelSpawn = deckedSpawn;
        deckedPump = null;
        deckedSpawn = null;
        UnqueuePump();
    }

    public void UnqueuePump()
    {
        myUI.PumpClear();
    }
}
