using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class Tile {

    // Custom Vector3 Class Supporting Serialization
    [System.Serializable]
    public class Vector3Serializable {
        private Vector3 vector = Vector3.zero;

        public float X { get { return vector.x; } set { vector.x = value; } }
        public float Y { get { return vector.y; } set { vector.y = value; } }
        public float Z { get { return vector.z; } set { vector.z = value; } }

        public Vector3Serializable() { }
        public Vector3Serializable(Vector3 vector) {
            this.vector = vector;
        }

        public static implicit operator Vector3 (Vector3Serializable arg) {
            return new Vector3(arg.X, arg.Y, arg.Z);
        }

        public static explicit operator Vector3Serializable (Vector3 arg) {
            return new Vector3Serializable(arg);
        }
    }


    [XmlIgnore]
    public GameObject prefab;

    [XmlAttribute("name")]
    public string Name { get; set; }

    public Vector3Serializable Position { get; set; }
    public Vector3Serializable Size { get; set; }
    public Vector3Serializable Rotation { get; set; }

    public Tile() { }
    public Tile(Transform transform) {
        prefab = transform.gameObject;
        Name = transform.name;
        Position = (Vector3Serializable) transform.localPosition;
        Size = (Vector3Serializable) transform.localScale;
        Rotation = (Vector3Serializable) transform.localEulerAngles;
    }

    public void Instantiate(string directory, Transform parent) {
        prefab = Object.Instantiate(Resources.Load<GameObject>(directory + Name), Position, Quaternion.Euler(Rotation), parent);
        prefab.name = Name;
        prefab.transform.localScale = Size;
    }
}
