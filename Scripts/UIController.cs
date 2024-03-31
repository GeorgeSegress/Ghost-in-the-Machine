using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Transform player;

    public TMP_Text wordsText;

    public List<TrackerPack> packs = new List<TrackerPack>();
    public GameObject ping;
    public GameObject PumpText;
    public Vector3 basePos;

    public RectTransform compass;
    public GameObject compassObj;
    public GameObject notePickupText;
    public GameObject NotesDeck;
    public TMP_Text countText;
    public TMP_Text viewingText;
    public TMP_Text textText;
    private int currNote = 0;
    private bool viewingNotes;
    private bool movableNotes = true;

    private float trackerTime = 1.0f;

    public GameObject[] fuelBars;
    public GameObject[] healthBars;

    private Note[] allNotes;
    private string priorWords = "";

    private Test1 voiceBox;

    private void Start()
    {
        voiceBox = GetComponent<Test1>();
    }

    public void FixedUpdate()
    {
        if(viewingNotes)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow) && movableNotes)
            {
                DisplayNoteSingular(-1);
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow) && movableNotes)
            {
                DisplayNoteSingular(1);
            }
        }
        compass.localPosition = new Vector3(-player.rotation.eulerAngles.y * 1.877777f + 338, 0, 0);
        trackerTime += Time.deltaTime;
        if (trackerTime >= 1.5f)
            TrackerPulse();
    }
    
    void TrackerPulse()
    {
        trackerTime = 0;
        foreach (TrackerPack pack in packs.ToList())
        {
            Vector3 d = pack.DistFromPlayer();

            if ((d.x * d.x + d.z * d.z) > 14400) // checking if the pack is further than 8 units away then removing or adding to the pulse
                packs.Remove(pack);
            else
            {
                GameObject hey = Instantiate(ping, Vector3.zero, Quaternion.identity);
                hey.transform.SetParent(transform);
                //.transform.SetAsFirstSibling();
                hey.GetComponent<RectTransform>().localPosition = new Vector3(d.x * .4f + basePos.x, d.z * .4f + basePos.y, 0);
                hey.GetComponent<Image>().sprite = pack.GetSprite();
            }
        }
    }

    public void AddTracker(TrackerPack newPack)
    {
        packs.Add(newPack);
    }

    public void UpdateFuel(float value)
    {
        for(int i = 0; i < fuelBars.Length; i++)
        {
            fuelBars[i].SetActive(value > (100 / fuelBars.Length) * i);
        }
    }

    public void UpdateHealth(float value)
    {
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].SetActive(value / 100 > (100 / healthBars.Length) * i);
        }
    }

    public void NoteAvailable(bool yes)
    {
        notePickupText.SetActive(yes);
    }

    public void ReceiveText(string words)
    {
        StopAllCoroutines();//DisplayText(priorWords));
        priorWords = words;
        StartCoroutine(DisplayText(words));
    }

    public IEnumerator DisplayText(string words)
    {
        compassObj.SetActive(false);
        wordsText.gameObject.SetActive(true);
        wordsText.text = "";
        int lines = 0;
        int totalLine = 0;
        SayTwoLines(words, 0);
        foreach(char c in words)
        {
            wordsText.text = wordsText.text + c;
            yield return new WaitForSeconds(.068f);
            if (c == '\n')
            {
                lines++;
                totalLine++;
                if (lines >= 2)
                {
                    SayTwoLines(words, totalLine);
                    wordsText.text = "";
                    lines = 0;
                }
            }
        }
        yield return new WaitForSeconds(1);
        compassObj.SetActive(true);
        wordsText.gameObject.SetActive(false);
    }

    public void SayTwoLines(string text, int start)
    {
        string lines = "";
        int line = 0;
        foreach(char c in text)
        {
            if ((line == start || line == start + 1) && c != '\n')
                lines += c;
            else if (c == '\n')
                line += 1;
        }
        voiceBox.SayString(lines);
    }

    public void DisplayNotes(Note[] unlockedNotes)
    {
        allNotes = unlockedNotes;
        NotesDeck.SetActive(true);
        int count = 0;
        for(int i = 0; i < allNotes.Length; i++)
        {
            if (allNotes[i].name != "")
                count++;
        }
        countText.text = "Notes Unlocked:\n" + count + "/11";
        DisplayNoteSingular(0);
        viewingNotes = true;
    }

    public void DisplayNoteSingular(int increment)
    {
        currNote += increment;
        StartCoroutine(Navigability());
        if (currNote < 0)
            currNote = 10;
        if (currNote > 10)
            currNote = 0;
        if (allNotes[currNote].name == "")
        {
            viewingText.text = "Current Note:\n" + (currNote + 1) + " - Not Unlocked";
            textText.text = "Not currently viewable";
        } else
        {
            viewingText.text = "Current Note:\n" + (currNote + 1) + " - " + allNotes[currNote].name;
            textText.text = allNotes[currNote].text;
        }    
    }

    IEnumerator Navigability()
    {
        movableNotes = false;
        yield return new WaitForSeconds(.07f);
        movableNotes = true;
    }

    public void RevivePlayer()
    {
        FindObjectOfType<Player>().Revive();
    }

    public void CloseNotes()
    {
        viewingNotes = false;
        NotesDeck.SetActive(false);
    }

    public void QueuePump()
    {
        PumpText.SetActive(true);
    }

    public void PumpClear()
    {
        PumpText.SetActive(false);
    }
}
