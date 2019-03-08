using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "DataScoreLinesCleared", menuName = "Scriptable Objects/ScoreLinesCleared")]
public class DataScoreLinesCleared : ScriptableObject {
    [Header("Starting from one row")]
    public List<int> LinesCleared = new List<int>();
}