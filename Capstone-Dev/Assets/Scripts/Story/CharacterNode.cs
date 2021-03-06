﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterNode{
    [XmlAttribute("name")]
    public string Name;

    [XmlElement("ID")]
    public string ID;

    [XmlArray("Childs")]
    [XmlArrayItem("Child")]
    public string[] Childs;

    [XmlArray("Parents")]
    [XmlArrayItem("Parent")]
    public string[] Parents;

    [XmlElement]
    public string State;

}
