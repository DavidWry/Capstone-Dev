using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Dialogue {

    [XmlElement("Name")]
    public string Name;

    [XmlElement("Content")]
    public string Content;

    [XmlElement("Emoji")]
    public string Emoji;
}
