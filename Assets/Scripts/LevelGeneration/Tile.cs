using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class Tile {

    [XmlIgnore]
    public GameObject prefab;

    [XmlAttribute("name")]
    public string Name { get; set; }

    [System.Serializable]
    public class Vector3Serializable {
        private Vector3 vector = Vector3.zero;

        public float X { get { return vector.x; } set { vector.x = value; } }
        public float Y { get { return vector.y; } set { vector.y = value; } }
        public float Z { get { return vector.z; } set { vector.z = value; } }
    }

    public Vector3Serializable Position;
    public Vector3Serializable Size;
}
