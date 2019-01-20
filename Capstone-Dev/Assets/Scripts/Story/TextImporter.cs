using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextImporter : MonoBehaviour {

    public Text TextBox;

    public TextAsset TextFile;
    public string[] textLines;
    private int endOfLine;

	// Use this for initialization
	void Start () {
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
	void Update () {
		
	}
}
