using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public float HighScore;
    public Dictionary<string, int> EnemyKilled = new Dictionary<string, int>();
}
