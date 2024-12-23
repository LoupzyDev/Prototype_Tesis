using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectsData", menuName = "ScriptableObjects/ObjectsSO", order = 4)]
public class ObjectsDatabaseSO : ScriptableObject {
    public List<ObjectData> objectsData;
}

[Serializable]
public class ObjectData {
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; set; } = Vector2Int.one; 
    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    public int Price;
}
