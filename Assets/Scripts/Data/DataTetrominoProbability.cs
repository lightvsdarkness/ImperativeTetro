using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "DataTetrominoProbability", menuName = "Scriptable Objects/TetrominoProbability")]
public class DataTetrominoProbability : ScriptableObject
{
    public List<TetrominoLogic> TetraminoListAll = new List<TetrominoLogic>();
    public List<float> TetraminoProbability = new List<float>();


    //protected virtual void Start() {
    //}

    [ContextMenu("UpdateColor")]
    public float GetTetrominoProbability(TetrominoLogic tetrominoLogic) {
        int index = TetraminoListAll.FindIndex(x => x.NameTetromino == tetrominoLogic.NameTetromino);
        if (index == -1 || TetraminoProbability.Count < index - 1) {
            Debug.LogWarning("Not all Tetromino listed in current TetrominoProbability data source", this);
            return 1.0f;
        }
        else {
            return TetraminoProbability[index];
        }
    }
}