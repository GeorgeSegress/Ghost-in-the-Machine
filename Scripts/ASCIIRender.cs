using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class ASCIIRender : MonoBehaviour
{
    public int def_x;
    public int def_y;

    public GUIStyle style;

    public TMP_Text text;

    // string of ordered characters defined in the editor to create the text-based "gradient"
    public char[] characters;
    public string displayText;

    public AnimationCurve grayCurve;

    public RenderTexture heldTexture;
    private Texture2D flatTexture;
    public RenderTexture activeText;

    private void Start()
    {
        flatTexture = new Texture2D(128, 72);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 128, 72), displayText, style);
    }

    public void Update()
    {
        RenderTexture.active = activeText;

        flatTexture = new Texture2D(128, 72);
        flatTexture.ReadPixels(new Rect(0, 0, 128, 72), 0, 0);
        // updates UI text to the rendered version
        text.text = ScreenConvert(flatTexture);
         
        RenderTexture.active = null;
    }

    // runs through received render texture to build a complex string that averages each pixel's greyscale version, converting it to a stored level
    private string ScreenConvert(Texture2D myText)
    {
        StringBuilder stringBuilder = new();
        
        for(int y = myText.height - 1; y >= 0; y--)
        {
            for(int x = 0; x < myText.width; x++)
            {
                float grayed = grayCurve.Evaluate(myText.GetPixel(x, y).grayscale);

                stringBuilder.Append(CharacterFromValue(grayed));
            }
            stringBuilder.Append("\n");
        }

        return stringBuilder.ToString();
    }

    // gets character based on greyscale value
    private char CharacterFromValue(float value)
    {
        return characters[(int)(value * (characters.Length - 1))];
    }
}
