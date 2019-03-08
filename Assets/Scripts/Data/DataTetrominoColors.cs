using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "DataTetrominoColors", menuName = "Scriptable Objects/TetrominoColors")]
public class DataTetrominoColors : ScriptableObject, ISerializationCallbackReceiver {
    [Header("Border")]
    [Tooltip("Default is black")]
    public DataColor BorderColor;
    [Header("Tetramino")]
    public List<TetrominoLogic> TetraminoListAll = new List<TetrominoLogic>();
    public List<DataColor> TetraminoColors = new List<DataColor>();

    [Space]
    [Header("Runtime values")]
    [NonSerialized]
    public DataColor RuntimeBorderColor;
    [NonSerialized]
    public List<TetrominoLogic> RuntimeTetraminoListAll = new List<TetrominoLogic>();
    [NonSerialized]
    public List<DataColor> RuntimeTetraminoColors = new List<DataColor>();

    //protected virtual void Start() {

    //}

    public DataColor GetBorderColor() {
        if (RuntimeBorderColor == null)
            RuntimeBorderColor = DataColor.CreateInstance<DataColor>();
        return RuntimeBorderColor;
    }


    public DataColor GetTetrominoColor(TetrominoLogic tetrominoLogic) {
        int index = TetraminoListAll.FindIndex(x => x.NameTetromino == tetrominoLogic.NameTetromino);

        if(index == -1)
        {
            Debug.LogWarning("Not all Tetromino listed in current TetraminoColors data source", this);

            return AddEntry(tetrominoLogic);
        }
        else if (TetraminoColors.Count < index - 1) { 
            Debug.LogWarning("Not all colors for Tetromino are listed in current TetraminoColors data source", this);

            TetraminoListAll.RemoveAt(index);

            return AddEntry(tetrominoLogic); ;
        }
        else
        {
            return TetraminoColors[index];
        }
    }

    private DataColor AddEntry(TetrominoLogic tetrominoLogic) {
        GameObject newTetromino = (GameObject)Resources.Load(ManagerTetroGame.I.TetrominoPrefabsPath + "/" + tetrominoLogic.NameTetromino, typeof(GameObject));
        TetraminoListAll.Add(newTetromino.GetComponent<TetrominoLogic>());
        var color = DataColor.CreateInstance<DataColor>();
        color.RandomizeColor();
        TetraminoColors.Add(color);
        return color;
    }

    public void OnBeforeSerialize() {
    }
    public void OnAfterDeserialize() {
        RuntimeTetraminoListAll = TetraminoListAll;
        RuntimeTetraminoColors = TetraminoColors;
        RuntimeBorderColor = BorderColor;
    }
}