using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

public class ManagerVisuals : SingletonManager<ManagerVisuals> {
    [Space]
    public List<SpriteRenderer> BorderGrid = new List<SpriteRenderer>();

    [Space]
    public DataTetrominoColors TetrominoColors;
    public string DefaultNameTetrominoColors = "TetrominoColorsDefault";

    [Space] public DataSprite DataTetromino;
    public Sprite TetrominoSprite;
    private Sprite _tetrominoSpriteDefault;
    public string TetrominoSpritesPath = "Sprites";
    public string DefaultNameSprite = "SquareSprite";

    private void Start() {
        if (TetrominoColors == null)
            TetrominoColors =
                (DataTetrominoColors) Resources.Load(ManagerTetroGame.I.TetrominoDataPath + "/" + DefaultNameTetrominoColors, typeof (ScriptableObject));
        if (_tetrominoSpriteDefault == null)
            _tetrominoSpriteDefault = (Sprite)Resources.Load(TetrominoSpritesPath + "/" + DefaultNameSprite, typeof(Sprite));
        if (TetrominoSprite == null)
            Debug.LogWarning("No Tetromino sprite isn't set in reserve field", this);

        UpdateTetrominoSprite();
        ManagerTetroGame.I.GameStarted += ConstructBorderGrid;
    }

    private void ConstructBorderGrid() {
        foreach (var cell in BorderGrid) {
            cell.color = TetrominoColors.BorderColor.Color;
        }
    }

    private void UpdateTetrominoSprite() {
        if (DataTetromino?.Sprite != null)
            TetrominoSprite = DataTetromino.Sprite;
        else
            TetrominoSprite = _tetrominoSpriteDefault;

        if(TetrominoSprite == null)
            Debug.LogError("No Tetromino sprite set at all", this);
    }

    public DataColor GetTetrominoColor(TetrominoLogic tetrominoLogic) {
        return TetrominoColors.GetTetrominoColor(tetrominoLogic);
    }

    public Sprite GetTetrominoSprite() {
        if (TetrominoSprite != null)
            return TetrominoSprite;

        if (DataTetromino?.Sprite != null) 
            return TetrominoSprite = DataTetromino.Sprite;

        if (_tetrominoSpriteDefault != null)
            return TetrominoSprite = _tetrominoSpriteDefault;

        Debug.LogWarning("Tetromino sprite isn't set in data source", this);
        return null;
    }
}