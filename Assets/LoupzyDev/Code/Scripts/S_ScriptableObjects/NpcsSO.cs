using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCsData", menuName = "ScriptableObjects/NPCsSO", order = 1)]
public class NpcsSO : ScriptableObject {
    public List<NPCData> npcList;
}

[Serializable]
public class NPCData {
    public string npcName;

    [Range(0, 100)]
    public int HP = 100;

    [Range(0, 100)]
    public int Stamina = 100;

    [Range(0, 100)]
    public int Moral = 100;

    [Range(0, 100)]
    public int Happiness = 100;

    [Range(0, 10)]
    public int Speed = 100;

    [Range(0, 100)]
    public int Quality = 100;

    public enum RoleType { Programmer, Artist, Designer }
    public RoleType Role;

    [Range(0, 10)]
    public int WorkLoad = 0;

    [Range(0, 10)]
    public int MinWorkLoad = 2;
}
