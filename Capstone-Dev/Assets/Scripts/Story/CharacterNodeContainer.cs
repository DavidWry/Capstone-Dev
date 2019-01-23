using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("CharacterTree")]
public class CharacterNodeContainer
{
    [XmlArray("CharacterNodes")]
    [XmlArrayItem("CharacterNode")]
    public List<CharacterNode> CharacterNodes = new List<CharacterNode>();

    public static CharacterNodeContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterNodeContainer));
        StringReader reader = new StringReader(_xml.text);
        CharacterNodeContainer CharacterNodes = serializer.Deserialize(reader) as CharacterNodeContainer;

        reader.Close();
        return CharacterNodes;
    }
}
