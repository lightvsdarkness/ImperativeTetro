using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TetrominoLogic : MonoBehaviour {
    public string NameTetromino;

    public bool AllowRotation = true;
    public bool LimitRotation;

    [Space]
    public float TImeOfLastFall = 0;
    public float SpeedFall = 1;

    public float InputSpeedHorizontal = 0.02f;
    public float InputSpeedVertical = 0.05f;
    public float InputTimerHorizontal;
    public float InputTimerVertical;

    private ManagerTetroGame _game;
    private TetrominoVisuals _visuals;
    private bool FallenThisUpdateCycle;

    private void Awake() {
        if (NameTetromino == "")
            NameTetromino = GetNameFromPrefabs();

        if (_visuals == null)
            _visuals = GetComponent<TetrominoVisuals>();
        if (_visuals == null)
            _visuals = gameObject.AddComponent<TetrominoVisuals>();
    }

    private void Update() {
        UpdateFallTimer();
    }

    public void Initialize(ManagerTetroGame game) {
        _game = game;
        _visuals.Initialize(this);
    }

    public void UpdateFallTimer() {
        if (Time.time - TImeOfLastFall >= SpeedFall) {
            TryMoveDown();
        }
    }

    public void TryMoveDown() {
        if (!FallenThisUpdateCycle) {
            MoveDown();
            FallenThisUpdateCycle = false;
        }
    }

    public void MoveRight() {
        transform.position += new Vector3(1, 0, 0);
        if (CheckValidPosition())
        {
            _game.UpdateGrid(this);
        }
        else
        {
            transform.position += new Vector3(-1, 0, 0);
        }
    }

    public void MoveLeft() {
        transform.position += new Vector3(-1, 0, 0);
        if (CheckValidPosition())
        {
            _game.UpdateGrid(this);
        }
        else
        {
            transform.position += new Vector3(1, 0, 0);
        }
    }

    public void Rotate() {
        if (AllowRotation)
        {
            if (LimitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
            if (CheckValidPosition())
            {
                _game.UpdateGrid(this);
            }
            else
            {
                if (LimitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                        transform.Rotate(0, 0, -90);
                    else
                        transform.Rotate(0, 0, 90);
                }
            }

        }
    }

    public void MoveDown() {
        transform.position += new Vector3(0, -1, 0);

        if (CheckValidPosition()) {
            _game.UpdateGrid(this);
        }
        else {
            transform.position += new Vector3(0, 1, 0);

            enabled = false;

            //
            _game.ProcessTetraminoDown(this);
        }

        TImeOfLastFall = Time.time;
    }

    [ContextMenu("GetNameFromPrefabs")]
    private string GetNameFromPrefabs() {
        return NameTetromino = gameObject.name;
    }

    private bool CheckValidPosition() {
        foreach (Transform square in transform) {
            Vector2 pos = _game.Round(square.position);

            if (_game.CheckPosInsideGrid(pos) == false)
                return false;

            if (_game.GetTransformAtGridPosition(pos) != null && _game.GetTransformAtGridPosition(pos).parent != transform)
                return false;
        }
        return true;
    }

}