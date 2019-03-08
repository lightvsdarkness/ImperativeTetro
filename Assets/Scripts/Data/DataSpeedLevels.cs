using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSpeedLevels", menuName = "Scriptable Objects/SpeedLevels")]
public class DataSpeedLevels : ScriptableObject {
    public List<Vector2> LevelsAndSpeed = new List<Vector2>();
    [Space]
    public float Coefficient = 1.33f;
    public int LevelsAmount = 10;

    //protected virtual void Start() {

    //}

    [ContextMenu("UpdateSpeedLevels")]
    public void UpdateSpeedLevels() {
        for (int i = 1; i <= LevelsAmount; i++) {
            LevelsAndSpeed.Add(new Vector2(i, 1 * Coefficient));
        }
    }
}
