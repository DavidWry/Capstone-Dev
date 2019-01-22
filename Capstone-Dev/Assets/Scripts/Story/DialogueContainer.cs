using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("Dialogue")]
public class DialogueContainer {
    [XmlArray("Lines")]
    [XmlArrayItem("Line")]
    public List<Dialogue> lines = new List<Dialogue>();

    public static DialogueContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(DialogueContainer));
        StringReader reader = new StringReader(_xml.text);
        DialogueContainer lines = serializer.Deserialize(reader) as DialogueContainer;

        reader.Close();
        return lines;
    }
}
