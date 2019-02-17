using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManagerXML : MonoBehaviour {

    public GameObject TextBox;
    public GameObject Player;
    public Text TextContent;
    public Text NameBox;

    public TextAsset TextFile;
    public string[] textLines;

    public bool isActive = false;

    [SerializeField]
    private int currentLine;
    [SerializeField]
    private int endOfLine;

    private bool isTyping = false;
    private bool cancelTyping = false;

    [SerializeField]
    private float typingSpeed = 0.02f;

    public const string path = "TestXML";
    private DialogueContainer dialogueContainer;

    // Use this for initialization
    void Start()
    {
        dialogueContainer = DialogueContainer.Load(path);      
        currentLine = 0;
        Player = GameObject.FindWithTag("Player");
        endOfLine = dialogueContainer.lines.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {

            if (Input.GetButtonDown("AButton"))
            {
                if (!isTyping)
                {
                    currentLine++;
                    if (currentLine > endOfLine)
                    {
                        DisableTextBox();
                    }
                    else
                    {
                        NameBox.text = dialogueContainer.lines[currentLine - 1].Name;
                        StartCoroutine(TextScroll(dialogueContainer.lines[currentLine - 1].Content));
                    }
                }
                else if (isTyping && !cancelTyping)
                {
                    cancelTyping = true;
                }
            }
            if (Input.GetButtonDown("BButton"))
            {
                DisableTextBox();
            }
        }

    }

    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        TextContent.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
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
        Player.GetComponent<Movement_New>().enabled = false;
        Player.GetComponent<Shoot_New>().enabled = false;
        isActive = true;
    }

    public void DisableTextBox()
    {
        currentLine = 0;
        TextBox.SetActive(false);
        Player.GetComponent<Movement_New>().enabled = true;
        Player.GetComponent<Shoot_New>().enabled = true;
        isActive = false;
    }

    public void ReloadText(string newTextPath)
    {
        dialogueContainer = DialogueContainer.Load(newTextPath);
        endOfLine = dialogueContainer.lines.Count;
    }
}
