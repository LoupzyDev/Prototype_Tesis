using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TaskSO : ScriptableObject{

    public string Name;

    public string Description;

    public string Type;

    public string[] Personal;

    public float Time;
}

