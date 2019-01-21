using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {
    public GameObject TextBox;
    public GameObject Player;
    public Text TextContent;

    public TextAsset TextFile;
    public string[] textLines;

    private int currentLine;
    private int endOfLine;

    private bool isTyping = false;
    private bool cancelTyping = false;

    [SerializeField]
    private float typingSpeed = 0.02f;

    // Use this for initialization
    void Start()
    {
        currentLine = 0;
        Player = GameObject.FindWithTag("Player");
        if (TextFile != null)
        {
            textLines = (TextFile.text.Split('\n'));
        }
        if (endOfLine == 0)
        {
            endOfLine = textLines.Length - 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TextContent.text = textLines[currentLine];

        if (Input.GetButtonDown("AButton"))
        {
            if (!isTyping)
            {
                currentLine ++;
                if (currentLine > endOfLine)
                {
                    DisableTextBox();
                }
                else
                {
                    StartCoroutine(TextScroll(textLines[currentLine]));
                }
            }
            else if (isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
        }

    }

    private IEnumerator TextScroll (string lineOfText)
    {
        int letter = 0;
        TextContent.text = "";
        isTyping = true;
        cancelTyping = false;
        while(isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            TextContent.text += lineOfText[letter];
            letter++;
            yield return new WaitForSeconds(typingSpeed);
        }
        TextContent.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox()
    {
        TextBox.SetActive(true);
        Player.GetComponent<Movement>().enabled = false;
        Player.GetComponent<Shoot>().enabled = false;
        //StartCoroutine(TextScroll(textLines[currentLine]));
    }

    public void DisableTextBox()
    {
        currentLine = 0;
        TextBox.SetActive(false);
        Player.GetComponent<Movement>().enabled = true;
        Player.GetComponent<Shoot>().enabled = true;
    }
}
