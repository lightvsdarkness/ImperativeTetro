using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "DataScore", menuName = "Scriptable Objects/Score")]
public class DataScore : ScriptableObject
{
    public int CurrentScore;
    public int HighScore;
}