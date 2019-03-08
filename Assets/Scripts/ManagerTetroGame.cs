using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using General;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ManagerTetroGame : SingletonManager<ManagerTetroGame> {
    [Header("If GridData is set manually, it's used instead of generation or GridWidth & GridHeight")]
    public DataGrid GridData;
    public int GridWidth = 10;
    public int GridHeight = 20;

    [Space]
    public float WaitTimeBeforeNextGame = 3;

    public Transform[,] Grid; // TODOMAYBE: Odin or Editor script

    public List<TetrominoLogic> TetrominoListForMatch = new List<TetrominoLogic>();
    public List<TetrominoLogic> TetrominoSpawnedAll = new List<TetrominoLogic>();
    public List<string> TetrominoSpawnedTraced = new List<string>(); // For game design stuff also, but not used really
    public int TraceTetrominoAmount = 5; // For game design stuff also, but not used really

    public Vector2Int TetrominoSpawnPos;

    [Header("UI")]
    public GameObject UIGameOver;
    [Header("Resources paths")]
    public string TetrominoPrefabsPath = "Prefabs";
    public string TetrominoDataPath = "Data";

    public TetrominoLogic CurrentTetromino;
    private bool Spawning = true;

    public event Action GameStarted; 
    public event Action GameOver;   //Add Highscore serialization 
    public event Action RowCleared;
    public event Action TetrominoMovedSideways;
    public event Action TetrominoMovedDown;
    public event Action TetrominoLanded;

    private void Start() {
        // Score
        ManagerScore.I.Initialize(this);

        // A little of UI
        if (UIGameOver != null)
            UIGameOver.SetActive(false);
        else 
            Debug.LogWarning("UIGameOver is missing, please set", this);

        if (TetrominoListForMatch.Count < 1)
            Debug.LogError("TetraminoPrefabs for a match wasn't set", this);

        if (GridData != null) { 
            GridWidth = GridData.GridWidth;
            GridHeight = GridData.GridHeight;
        }
        Grid = new Transform[GridWidth, GridHeight];

        if (TetrominoSpawnedTraced.Count == 0) {
            GameStarted?.Invoke();
            SpawnTetromino();
        }
    }

    public void Update() {

    }

    public void UpdateGrid(TetrominoLogic tetrominoLogic) {
        for (int y = 0; y < GridHeight; ++y) {
            for (int x = 0; x < GridWidth; ++x) {
                if (Grid[x, y] != null)
                {
                    if (Grid[x, y].parent == tetrominoLogic.transform) {
                        Grid[x, y] = null;
                    }
                }

            }
        }

        foreach (Transform square in tetrominoLogic.transform) {
            Vector2 pos = Round(square.position);
            if (pos.y < GridHeight) {
                Grid[(int) pos.x, (int) pos.y] = square;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector2 pos) {
        if (pos.y > GridHeight - 1) return null;
        return Grid[(int)pos.x, (int)pos.y];
    }

    public void ProcessTetraminoDown(TetrominoLogic tetramino) {
        TryDeleteRow();

        if (CheckRowAboveGrid(tetramino)) {
            GameLost();
        }
        
        CurrentTetromino = null;
        
        SpawnTetromino();

        RemoveTetrominoFromListSpawnedAll();
        RemoveTetrominoFromListSpawnedTraced();
    }

    private void RemoveTetrominoFromListSpawnedAll() {
        List<int> indicesToDelete = new List<int>();

        for (int i = 0; i < TetrominoSpawnedAll.Count; i++) {
            if (TetrominoSpawnedAll[i].transform.childCount == 0)
            {
                Destroy(TetrominoSpawnedAll[i].gameObject);
                indicesToDelete.Add(i);
            }
        }
        foreach (int indice in indicesToDelete.OrderByDescending(v => v)) {
            TetrominoSpawnedAll.RemoveAt(indice);
        }
    }

    private void RemoveTetrominoFromListSpawnedTraced() {
        if (TetrominoSpawnedTraced.Count > TraceTetrominoAmount) {
            TetrominoSpawnedTraced.RemoveAt(0);
        }

    }

    private void SpawnTetromino() {
        if (!Spawning) return;

        GameObject tetromino = (GameObject)Resources.Load(TetrominoPrefabsPath + "/" + RandomizeTetromino().NameTetromino, typeof(GameObject));
        TetrominoSpawnPos = new Vector2Int(GridWidth / 2, GridHeight - 1);
        var tetrominoLogic = Instantiate(tetromino, new Vector2(TetrominoSpawnPos.x, TetrominoSpawnPos.y), Quaternion.identity).GetComponent<TetrominoLogic>();
        tetrominoLogic.Initialize(this);
        TetrominoSpawnedAll.Add(tetrominoLogic);
        TetrominoSpawnedTraced.Add(tetrominoLogic.NameTetromino);

        CurrentTetromino = tetrominoLogic;
    }

    private bool CheckRowAboveGrid(TetrominoLogic tetromino) {
        for (int x = 0; x < GridWidth; ++x) {
            foreach (Transform square in tetromino.transform) {
                Vector2 pos = Round(square.position);
                if (pos.y > GridHeight - 1)
                    return true;
            }
        }
        return false;
    }

    public bool CheckPosInsideGrid(Vector2 pos) {
        return (int)pos.x >= 0 && (int)pos.x <= GridWidth - 1 && (int)pos.y >= 0;
    }

    private bool CheckRowFull(int y) {
        for (int x = 0; x < GridWidth; ++x) {
            if (Grid[x, y] == null)
                return false;
        }

        RowCleared?.Invoke();
        return true;
    }

    private void TryDeleteRow() {
        for (int y = 0; y < GridHeight; ++y)
        {
            if (CheckRowFull(y)) {
                DeleteSquareAt(y);
                MoveAllRowsDown(y + 1);
                --y;
            }
        }
    }

    private void DeleteSquareAt(int y) {
        for (int x = 0; x < GridWidth; ++x) {
            Destroy(Grid[x, y].gameObject);
            Grid[x, y] = null;
        }
    }

    private void MoveAllRowsDown(int y) {
        for (int i = y; i < GridHeight; ++i) {
            MoveRowDown(i);
        }
    }

    private void MoveRowDown(int y) {
        for (int x = 0; x < GridWidth; ++x) {
            if (Grid[x, y] != null) {
                Grid[x, y - 1] = Grid[x, y];
                Grid[x, y] = null;
                Grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public Vector2 Round(Vector2 pos) {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    private TetrominoLogic RandomizeTetromino() {
        int randomIndex = Random.Range(0, TetrominoListForMatch.Count - 1);
            return TetrominoListForMatch[randomIndex];
    }

    public void GameLost() {
        Spawning = false;

        if (UIGameOver != null)
            UIGameOver.SetActive(true);

        // Ad goes here


        Invoke("GameOver", WaitTimeBeforeNextGame);
    }

    /// <summary>
    /// If user doens't want to pay - it's the end of the world
    /// NOTE: Do not rename, as it's being invoked
    /// </summary>
    private void GameOver() {
        SceneManager.LoadScene(0);
    }
}