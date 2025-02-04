using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DaysData", menuName = "ScriptableObjects/DaySO", order = 3)]

public class DaySO : ScriptableObject{
    public List<DayData> days;
}

[Serializable]
public class DayData
{
    public int numberOfTasks;

    public float minQuality;

}
