using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("TileCollection")]
public class TileContainer {

    [XmlArray("Tiles"), XmlArrayItem("Tile")]
    public List<Tile> tileList = new List<Tile>();

    private string levelName;
    private static Encoding encoding = Encoding.GetEncoding("UTF-8");

    public void Save(string path) {
        var serializer = new XmlSerializer(typeof(TileContainer));
        using (var stream = new StreamWriter(path, false, encoding)) {
            serializer.Serialize(stream, this);
        }
    }

    public static TileContainer Load(string path) {
        var serializer = new XmlSerializer(typeof(TileContainer));
        using (var stream = new StreamReader(path, encoding)) {
            return serializer.Deserialize(stream) as TileContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static TileContainer LoadFromText(string text) {
        var serializer = new XmlSerializer(typeof(TileContainer));
        return serializer.Deserialize(new StringReader(text)) as TileContainer;
    }
}
