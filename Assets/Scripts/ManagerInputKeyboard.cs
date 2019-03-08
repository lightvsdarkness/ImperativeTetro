using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

public class ManagerInputKeyboard : SingletonManager<ManagerInputKeyboard>
{
    public float InputSpeedHorizontal = 0.02f;
    public float InputSpeedVertical = 0.05f;
    public float InputSpeedRotation = 0.05f;

    public float InputTimerHorizontal;
    public float InputTimerVertical;
    public float InputTimerRotation;

    //private void Start() {
    //}

#if UNITY_STANDALONE
    private void Update() {
        CheckUserInput();
    }
#endif

    public void CheckUserInput() {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (InputTimerHorizontal < InputSpeedHorizontal)
            {
                InputTimerHorizontal += Time.deltaTime;
                return;
            }
            InputTimerHorizontal = 0;

            if (ManagerTetroGame.I.CurrentTetromino != null)
                ManagerTetroGame.I.CurrentTetromino.MoveRight();
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (InputTimerHorizontal < InputSpeedHorizontal)
            {
                InputTimerHorizontal += Time.deltaTime;
                return;
            }
            InputTimerHorizontal = 0;

            if (ManagerTetroGame.I.CurrentTetromino != null)
                ManagerTetroGame.I.CurrentTetromino.MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //if (InputTimerRotation < InputSpeedRotation)
            //{
            //    InputTimerRotation += Time.deltaTime;
            //    return;
            //}
            //InputTimerRotation = 0;

            if (ManagerTetroGame.I.CurrentTetromino != null)
                ManagerTetroGame.I.CurrentTetromino.Rotate();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (InputTimerVertical < InputSpeedVertical)
            {
                InputTimerVertical += Time.deltaTime;
                return;
            }
            InputTimerVertical = 0;

            if (ManagerTetroGame.I.CurrentTetromino != null)
                ManagerTetroGame.I.CurrentTetromino.TryMoveDown();
        }

    }
}